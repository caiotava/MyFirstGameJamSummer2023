using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Spawner : MonoBehaviour
{
    


public List<GameObject> prefab;
    public List<Transform> spawnPoints;
    public float spawnInterval= 3f;


    void Start(){
        StartSpawning();
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnDelay());
    }

   IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnEnemy();
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnDelay());
        
    }

    private void SpawnEnemy()
    {
        int RandomPrefabID = Random.Range(0,prefab.Count);

        int RandomSpawnPoint = Random.Range(0, spawnPoints.Count);

        Instantiate(prefab[RandomPrefabID], spawnPoints[RandomSpawnPoint]);


    }
}


   