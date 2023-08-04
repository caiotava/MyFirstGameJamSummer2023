using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


    public class Enemy : MonoBehaviour
{

    Animator myAnimation;
    
    Player[] player;
    Rigidbody2D rb;
    public bool ischasing;
    public float speed = 2f;
    public float isInRange = 2f;
    private float timeSinceLastAttack = 0;
    private float timeBetweenattacks = 1f;
    public float health = 100;
    public int EnemyDamage = 3;



    // Start is called before the first frame update
    void Awake()
    {
     myAnimation = GetComponent<Animator>();
      player = FindObjectsOfType<Player>();
      rb = GetComponent<Rigidbody2D>();
        
           
    }

    // Update is called once per frame
     private void Update() {
        timeSinceLastAttack += Time.deltaTime;
       if(player == null) return;
       for(int i = 0; i < player.Count(); i++){
        if(player != null ) Chase(i);    
       }
       
        
    }

    

    public void Chase(int i)
    {
      {
        if(player[i]== null) return;
        if((Vector2.Distance(transform.position, player[i].transform.position )> isInRange)){
            MoveToPlayer(i);
            myAnimation.SetBool("walk", true);
            ischasing = true;
          
            }
            if(Vector2.Distance(transform.position, player[i].transform.position )< isInRange)
        {
            Attack(i); 
           
        }
         if(player[i].health == 0)
          { 
            myAnimation.SetBool("Attack",false);
           }

    }}

    private void Attack(int i)
    {
        ischasing = false;
        myAnimation.SetBool("walk", false);

        if (timeSinceLastAttack > timeBetweenattacks)
        {

            player[i].TakeDamage(EnemyDamage);
            timeSinceLastAttack = 0;
            myAnimation.SetBool("Attack", true);

        }
        
    }

    private void MoveToPlayer(int i)
    {
      
 transform.position = Vector3.MoveTowards(transform.position, player[i].transform.position, Time.deltaTime * speed);
     
       
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, isInRange);
    }
    public void GiveDamage( float damage){
        health = Mathf.Max(health - damage, 0);
         if(health == 0){
        
          
            GameObject.Destroy(this.gameObject);
        }
    }
     
}
    
    

    

    





