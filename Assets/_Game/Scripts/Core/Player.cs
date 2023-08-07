using System.Collections;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] public Unit unit;

    private float health;
    private float timeSinceLastAttack;

    private Enemy[] enemy;
    private Animator myAnimation;

    // Start is called before the first frame update
    public void Start()
    {
        myAnimation = GetComponent<Animator>();
        health = unit.unitStats.Health;
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
        Gizmos.DrawWireSphere(transform.position, unit.unitStats.AttackRange);
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

        if (timeSinceLastAttack > unit.unitStats.AttackSpeed)
        {
            if (enemy[i] is not null && Vector2.Distance(transform.position, enemy[i].transform.position) <=
                unit.unitStats.AttackRange)
            {
                myAnimation.SetBool("Attack", true);
                enemy[i].GiveDamage(unit.unitStats.Attack);
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

        if (health != 0)
        {
            return;
        }

        unit.OnUnitKill.Invoke(unit.unitStats);
        Destroy(gameObject);
    }
}