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
    [SerializeField] private GameObject selectedSprite;
    public UnitStats unitStats { get; private set; }
    public UnityEvent<UnitStats> OnUnitKill;
    public UnityEvent<UnitStats, float> OnUniHit;

    private float health;
    private RaycastHit2D target;
    private Unit targetUnit;
    private bool isChasing;
    private float timeSinceLastAttack;
    private bool isMelee;
    private Vector3 movePosition;

    public void InitializeUnitStats(UnitStats stats)
    {
        unitStats = stats;
        health = unitStats.Health;
        isMelee = bulletPrefab is null;
        SetSelected(false);
        movePosition = transform.position;
    }

    private void Update()
    {
        if (movePosition != transform.position)
        {
            attackAnimation.SetBool("Walk", true);
            transform.position = Vector3.MoveTowards(
                transform.position,
                movePosition,
                Time.deltaTime * unitStats.MovingSpeed
            );

            spriteRenderer.flipX = transform.position.x < movePosition.x;
            return;
        }

        attackAnimation.SetBool("Walk", false);

        if (!findTarget())
        {
            attackAnimation.SetBool("Idle", true);
            return;
        }

        chase();
    }

    private bool findTarget()
    {
        if (targetUnit != null && !isMelee)
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
        if (targetUnit == null)
        {
            return false;
        }

        spriteRenderer.flipX = transform.position.x < target.transform.position.x;
        return true;
    }

    private void chase()
    {
        if (allowChasing && Vector2.Distance(transform.position, targetUnit.transform.position) > unitStats.AttackRange)
        {
            movePosition = target.transform.position;
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

        if (health > 0)
        {
            return;
        }

        Destroy(gameObject);
        OnUnitKill.Invoke(unitStats);
    }

    public void SetSelected(bool isSelected)
    {
        if (selectedSprite != null)
        {
            selectedSprite.SetActive(isSelected);
        }
    }

    public void SetMovePosition(Vector3 position)
    {
        movePosition = position;
    }
}