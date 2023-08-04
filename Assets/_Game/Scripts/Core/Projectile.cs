using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Enemy enemy;
    Player player;
   
    public GameObject  Prefab;

    
    public float nextFireTime;
    public float fireRate = 2f;
    public float force;
    

    public void Start()
    {
        enemy = FindAnyObjectByType<Enemy>();
          player = FindAnyObjectByType<Player>();
    }

    public void FixedUpdate()
    {
        if(enemy == null) return;
       
        if((Vector2.Distance(transform.position, enemy.transform.position )<= player.Range && nextFireTime <= Time.time)){
            var bullet =Instantiate(Prefab,transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
            Destroy(bullet, 1f);

        }   
    }
   
  


    
 }

