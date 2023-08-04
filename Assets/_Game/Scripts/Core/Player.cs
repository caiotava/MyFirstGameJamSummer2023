using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    Animator myAnimation;
    Enemy[] enemy;

    

    public float Range = 2f;
    
    public int PlayerDamage = 5;
    public float timeBetweenattacks = 1f;
    float timeSinceLastAttack = 0f;
    public float health = 100;
    

    // Start is called before the first frame update
    public void Awake()
    {
   
        enemy = FindObjectsOfType<Enemy>();
        myAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {   
        timeSinceLastAttack += Time.deltaTime;
        if(enemy == null)return;
        else{
        for(int i = 0; i < enemy.Count(); i++){   
          PlayerAttack(i);     
        }
        }
        
    }

     private void PlayerAttack(int i)
    {
        if(enemy != null){

        if(enemy[i] == null) return;
        if(timeSinceLastAttack > timeBetweenattacks){
             if((Vector2.Distance(transform.position, enemy[i].transform.position )<= Range)){
            myAnimation.SetBool("Attack", true);
            enemy[i].GiveDamage(PlayerDamage);
            timeSinceLastAttack = 0;
            print(i);
        }}
         if(enemy[i].health <= 0)
          { 
            myAnimation.SetBool("Attack",false);
           }
        }
        print(i);
       
        

    }








   

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
    public void TakeDamage(int damage){
        health = Mathf.Max(health - damage, 0);
        if(health == 0)
        {
            GameObject.Destroy(this.gameObject);
            
        }

    }

    
}
