using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private UnitStats unitEnemy;
    [SerializeField] private GameObject enemiesGroup;
    [SerializeField] private float spawnIntervalMin;
    [SerializeField] private float spawnIntervalMax;
    [SerializeField] private int initialEnemySpawn = 1;
    [SerializeField] private int incrementalEnemySpawn = 2;
    [SerializeField] private int maxEnemiesToSpawn = 10;

    private int totalEnemiesToSpawn;

    private void Start()
    {
        totalEnemiesToSpawn = initialEnemySpawn;
    }

    public void StartSpawn()
    {
        totalEnemiesToSpawn += Math.Min(totalEnemiesToSpawn * incrementalEnemySpawn, maxEnemiesToSpawn);
        StartCoroutine(spawnEnemyRountine());
    }

    private IEnumerator spawnEnemyRountine()
    {
        while (enemiesGroup.transform.childCount <= totalEnemiesToSpawn)
        {
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            var position = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            var unit = Instantiate(unitEnemy.unitObject, position.transform.position, Quaternion.identity,
                enemiesGroup.transform);

            unit.InitializeUnitStats(unitEnemy);
        }
    }

    public void ClearAllUnities()
    {
        foreach (Transform child in enemiesGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
}