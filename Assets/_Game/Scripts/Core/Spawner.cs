using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Spawner : MonoBehaviour
{
    

    Enemy[] enemy;
    public List<GameObject> prefab;
    public List<Transform> spawnPoints;
    public float spawnInterval= 3f;
    public float enemyToSpawn = 10;
    public int enemySpawned;


    void Start(){
        StartSpawning();
    }
    void Update(){
        enemy = FindObjectsOfType<Enemy>();
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnDelay());
        
      
    }

   IEnumerator SpawnDelay()
    {

         if(enemySpawned <= enemyToSpawn){
            enemySpawned =  enemySpawned+ 1;
        yield return new WaitForSeconds(spawnInterval);
        SpawnEnemy();
        yield return new WaitForSeconds(spawnInterval);
        
        StartCoroutine(SpawnDelay());
        
         }
        else{
          StopCoroutine(SpawnDelay());  
        }
        
        
    }

    private void SpawnEnemy()
    {
        int RandomPrefabID = Random.Range(0,prefab.Count);

        int RandomSpawnPoint = Random.Range(0, spawnPoints.Count);

        Instantiate(prefab[RandomPrefabID], spawnPoints[RandomSpawnPoint]);


    }
}


   