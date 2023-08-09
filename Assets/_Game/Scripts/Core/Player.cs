using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Unit unit;

    [SerializeField] public SpawnManager spawnManager;

    private float health;
    private float timeSinceLastAttack;

    private Enemy[] enemies;
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
        timeSinceLastAttack += Time.deltaTime;

        foreach (var enemy in spawnManager.GetEnemies())
        {
            PlayerAttack(enemy);
        }
    }

    private void PlayerAttack(Enemy enemy)
    {
        if (timeSinceLastAttack > unit.unitStats.AttackSpeed)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= unit.unitStats.AttackRange)
            {
                myAnimation.SetBool("Attack", true);
                enemy.GiveDamage(unit.unitStats.Attack);
                timeSinceLastAttack = 0;
            }
        }

        if (enemy.unit.unitStats.Health <= 0)
        {
            myAnimation.SetBool("Attack", false);
        }
    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);

        unit.OnUniHit.Invoke(unit.unitStats, health);

        if (health != 0)
        {
            return;
        }

        unit.OnUnitKill.Invoke(unit.unitStats);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, unit.unitStats.AttackRange);
    }
}