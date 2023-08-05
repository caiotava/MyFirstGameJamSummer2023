using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    
    Enemy[] enemy;
    Player player;
    Vector3 targetpos;
    public float force = 5f;
    Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
          player = FindAnyObjectByType<Player>();
    }
     public void Update() {
        enemy = FindObjectsOfType<Enemy>();
         for(int i = 0; i < enemy.Count(); i++){
            if(enemy[i] == null ) return;
            if(player == null) return;
         targetpos = enemy[i].transform.position;
        Vector3 direction = (targetpos  - player.transform.position);
        rb.velocity = new Vector2(direction.x, direction.y) * force;
         }
      
    }
}
