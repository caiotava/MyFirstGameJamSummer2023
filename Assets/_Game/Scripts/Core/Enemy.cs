using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isChasing;
    public float speed = 2f;
    public float isInRange = 2f;
    public float health = 100;
    public int EnemyDamage = 3;
    private Animator myAnimation;
    private Player[] player;
    private Rigidbody2D rb;
    private readonly float timeBetweenattacks = 1f;
    private float timeSinceLastAttack;

    // Start is called before the first frame update
    private void Awake()
    {
        myAnimation = GetComponent<Animator>();
        player = FindObjectsOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (player == null)
        {
            return;
        }

        for (var i = 0; i < player.Count(); i++)
        {
            if (player != null)
            {
                Chase(i);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, isInRange);
    }


    public void Chase(int i)
    {
        {
            if (player[i] is null)
            {
                return;
            }

            if (Vector2.Distance(transform.position, player[i].transform.position) > isInRange)
            {
                MoveToPlayer(i);
                myAnimation.SetBool("walk", true);
            }

            if (Vector2.Distance(transform.position, player[i].transform.position) < isInRange)
            {
                Attack(i);
            }

            if (player[i].health == 0)
            {
                myAnimation.SetBool("Attack", false);
            }
        }
    }

    private void Attack(int i)
    {
        myAnimation.SetBool("walk", false);


        if (!(timeSinceLastAttack > timeBetweenattacks))
        {
            return;
        }

        player[i].TakeDamage(EnemyDamage);
        timeSinceLastAttack = 0;
        myAnimation.SetBool("Attack", true);
    }

    private void MoveToPlayer(int i)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player[i].transform.position,
            Time.deltaTime * speed
        );
    }

    public void GiveDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}