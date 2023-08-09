using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Unit unit;

    public bool isChasing;
    private Animator myAnimation;
    private Rigidbody2D rb;

    private float health = 100;
    private readonly float timeBetweenattacks = 1f;
    private float timeSinceLastAttack;


    // Start is called before the first frame update
    private void Start()
    {
        myAnimation = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        health = unit.unitStats.Health;
    }

    // Update is called once per frame
    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        foreach (var player in FindObjectsOfType<Player>())
        {
            Chase(player);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, unit.unitStats.AttackRange);
    }


    private void Chase(Player player)
    {
        {
            if (Vector2.Distance(transform.position, player.transform.position) > unit.unitStats.AttackRange)
            {
                MoveToPlayer(player);
                myAnimation.SetBool("walk", true);
            }

            if (Vector2.Distance(transform.position, player.transform.position) < unit.unitStats.AttackRange)
            {
                Attack(player);
            }

            if (player.unit.unitStats.Health == 0)
            {
                myAnimation.SetBool("Attack", false);
            }
        }
    }

    private void Attack(Player player)
    {
        myAnimation.SetBool("walk", false);


        if (!(timeSinceLastAttack > timeBetweenattacks))
        {
            return;
        }

        player.TakeDamage(unit.unitStats.Attack);
        timeSinceLastAttack = 0;
        myAnimation.SetBool("Attack", true);
    }

    private void MoveToPlayer(Component player)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.transform.position,
            Time.deltaTime * unit.unitStats.MovingSpeed
        );
    }

    public void GiveDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        unit.OnUniHit.Invoke(unit.unitStats, health);

        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}