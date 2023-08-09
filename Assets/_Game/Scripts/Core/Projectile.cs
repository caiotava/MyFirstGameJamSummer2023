using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Prefab;
    public float fireRate = 2f;
    public float DestroyTime = 1f;
    private Enemy[] enemy;
    private float nextFireTime;
    private Player player;

    public void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    public void FixedUpdate()

    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            BulletInstantiate(enemy);
        }
    }

    public void BulletInstantiate(Enemy enemy)
    {
        if (Vector2.Distance(transform.position, enemy.transform.position) <= player.unit.unitStats.AttackRange &&
            nextFireTime <= Time.time)
        {
            var bullet = Instantiate(Prefab, transform.position, Quaternion.identity, this.transform);
            nextFireTime = Time.time + fireRate;
            Destroy(bullet, DestroyTime);
        }
    }
}