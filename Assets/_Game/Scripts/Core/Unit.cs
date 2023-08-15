using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator attackAnimation;
    [SerializeField] private BulletTarget bulletPrefab;
    [SerializeField] private bool allowChasing = false;
    [SerializeField] private LayerMask targetLayer;
    public UnitStats unitStats { get; private set; }
    public UnityEvent<UnitStats> OnUnitKill;
    public UnityEvent<UnitStats, float> OnUniHit;

    private float health;
    private RaycastHit2D target;
    private Unit targetUnit;
    private bool isChasing;
    private float timeSinceLastAttack;
    private bool isMelee;
    private bool hasBeenChased;

    public void InitializeUnitStats(UnitStats stats)
    {
        unitStats = stats;
        health = unitStats.Health;
        isMelee = bulletPrefab is null;
    }

    private void Update()
    {
        if (tag == "Player")
        {
            var a = attackAnimation.enabled;
        }

        if (!findTarget())
        {
            attackAnimation.enabled = false;
            return;
        }

        chase();
    }

    private bool findTarget()
    {
        if (targetUnit is not null && !targetUnit.Equals(null) && !isMelee)
        {
            return true;
        }

        var pos = (Vector2)transform.position;
        var hits = Physics2D.CircleCastAll(pos, unitStats.ChaseRange, pos, 0.0f, targetLayer);

        isChasing = hits.Length > 0;

        if (!isChasing)
        {
            return false;
        }

        target = hits[0];
        targetUnit = target.transform.GetComponent<Unit>();
        foreach (var hit in hits)
        {
            targetUnit = hit.transform.GetComponent<Unit>();

            if (targetUnit.hasBeenChased)
            {
                continue;
            }

            target = hit;
            break;
        }

        if (targetUnit == null)
        {
            return false;
        }

        targetUnit.hasBeenChased = true;
        spriteRenderer.flipX = transform.position.x < target.transform.position.x;
        return true;
    }

    private void chase()
    {
        if (allowChasing && Vector2.Distance(transform.position, targetUnit.transform.position) > unitStats.AttackRange)
        {
            MoveToTarget();
            attackAnimation.SetBool("walk", true);
            return;
        }

        if (Vector2.Distance(transform.position, targetUnit.transform.position) < unitStats.AttackRange)
        {
            unitAttack();
            return;
        }

        targetUnit = null;
        target = new RaycastHit2D();
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            Time.deltaTime * unitStats.MovingSpeed
        );
    }

    private void unitAttack()
    {
        timeSinceLastAttack -= Time.deltaTime;
        if (timeSinceLastAttack > 0)
        {
            //attackAnimation.SetBool("Attack", false);
            return;
        }

        attackAnimation.SetBool("Attack", true);
        attackAnimation.enabled = true;

        if (!isMelee)
        {
            var project = Instantiate(bulletPrefab, transform.position, Quaternion.identity, transform);
            project.Initialize(this, targetUnit);
        }
        else
        {
            targetUnit.GiveDamage(unitStats.Attack);
        }

        timeSinceLastAttack = unitStats.AttackSpeed;
    }

    public void GiveDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        OnUniHit.Invoke(unitStats, health);

        if (!(health <= 0))
        {
            return;
        }

        Destroy(gameObject);
        OnUnitKill.Invoke(unitStats);
    }
}