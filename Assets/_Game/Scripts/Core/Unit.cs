using UnityEngine;
using UnityEngine.Events;

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
    private bool isMoving;
    private bool isChasing;
    private bool isMelee;
    private float timeSinceLastAttack;
    private Vector3 movePosition;

    private LineRenderer line;

    void StartLine()
    {
        line = gameObject.AddComponent<LineRenderer>(); // Add the LineRenderer
        line.material = new Material(Shader.Find("Sprites/Default")); // Set the material (optional)
        line.startColor = Color.red; // Set start color
        line.endColor = Color.blue; // Set end color
        line.startWidth = 0.1f; // Set start width
        line.endWidth = 0.1f; // Set end width
        line.positionCount = 2; // Set the number of points to the LineRenderer
        line.enabled = false;
    }

    public void InitializeUnitStats(UnitStats stats)
    {
        unitStats = stats;
        health = unitStats.Health;
        isMelee = bulletPrefab is null;
        SetSelected(false);
        movePosition = transform.position;

        StartLine();
    }

    private void Update()
    {
        line.enabled = false;

#if UNITY_EDITOR
        if (targetUnit != null && allowChasing)
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targetUnit.transform.position);
        }
#endif

        if (allowChasing && findTarget())
        {
            chase();
        }

        var newPos = new Vector3(movePosition.x, movePosition.y, transform.position.y);
        isMoving = newPos != transform.position;
        attackAnimation.SetBool("Walk", isMoving);
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                newPos,
                Time.deltaTime * unitStats.MovingSpeed
            );

            return;
        }

        if (!findTarget())
        {
            attackAnimation.SetBool("Idle", true);
            return;
        }

        chase();
    }

    private void LateUpdate()
    {
        if (isMoving)
        {
            spriteRenderer.flipX = transform.position.x < movePosition.x;
        }
        else if (targetUnit != null)
        {
            spriteRenderer.flipX = transform.position.x < targetUnit.transform.position.x;
        }
    }

    private bool findTarget()
    {
        if (targetUnit != null && !isMelee)
        {
            return true;
        }

        var pos = (Vector2)transform.position;
        var hits = Physics2D.CircleCastAll(pos, unitStats.ChaseRange, Vector2.zero, Mathf.Infinity, targetLayer);

        var minDistance = Mathf.Infinity;
        // Iterate over all the hit objects and find the closest
        foreach (var hit in hits)
        {
            var distance = (hit.point - (Vector2)transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                target = hit;
            }
        }

        isChasing = hits.Length > 0;
        if (!isChasing)
        {
            return false;
        }

        targetUnit = target.transform.GetComponent<Unit>();
        if (targetUnit == null)
        {
            return false;
        }

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
        movePosition = new Vector3(position.x, position.y, transform.position.z);
        targetUnit = null;
        target = new RaycastHit2D();
    }
}