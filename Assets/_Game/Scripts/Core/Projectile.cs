using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Enemy[] enemy;
    Player player;
    public GameObject  Prefab;
    float nextFireTime;
    public float fireRate = 2f;
    public float DestroyTime = 1f;

    public void Start()
    {
        enemy = FindObjectsOfType<Enemy>();
          player = FindAnyObjectByType<Player>();
    }

    public void FixedUpdate()
    {
        for(int i = 0; i < enemy.Length; i++){
            BulletInstantiate(i);
        }
        
    }

    private void BulletInstantiate(int i)
    {
        if (enemy[i] == null) return;

        if ((Vector2.Distance(transform.position, enemy[i].transform.position) <= player.Range && nextFireTime <= Time.time))
        {
            var bullet = Instantiate(Prefab, transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
            Destroy(bullet, DestroyTime);

        }
    }




}

