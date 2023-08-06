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
        enemy = FindObjectsOfType<Enemy>();
        for (var i = 0; i < enemy.Length; i++) BulletInstantiate(i);
    }

    public void BulletInstantiate(int i)
    {
        if (enemy[i] is null)
        {
            return;
        }

        if (Vector2.Distance(transform.position, enemy[i].transform.position) <= player.Range &&
            nextFireTime <= Time.time)
        {
            var bullet = Instantiate(Prefab, transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
            Destroy(bullet, DestroyTime);
        }
    }
}