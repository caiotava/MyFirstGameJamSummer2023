using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public float Range = 2f;
    public int PlayerDamage = 5;
    public float timeBetweenAttacks = 1f;
    public float health = 100;
    private Enemy[] enemy;
    private Animator myAnimation;

    private float timeSinceLastAttack;

    // Start is called before the first frame update
    public void Awake()
    {
        myAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        StartCoroutine(EnemyComponent());
        timeSinceLastAttack += Time.deltaTime;
        if (enemy == null)
        {
            return;
        }

        for (var i = 0; i < enemy.Count(); i++)
        {
            PlayerAttack(i);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    private IEnumerator EnemyComponent()
    {
        yield return new WaitForSeconds(2);
        enemy = FindObjectsOfType<Enemy>();
        yield return new WaitForSeconds(3);
        StartCoroutine(EnemyComponent());
    }

    private void PlayerAttack(int i)
    {
        if (enemy is null)
        {
            return;
        }

        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            if (enemy[i] is not null && Vector2.Distance(transform.position, enemy[i].transform.position) <= Range)
            {
                myAnimation.SetBool("Attack", true);
                enemy[i].GiveDamage(PlayerDamage);
                timeSinceLastAttack = 0;
            }
        }

        if (enemy[i] is not null && enemy[i].health <= 0)
        {
            myAnimation.SetBool("Attack", false);
        }
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}