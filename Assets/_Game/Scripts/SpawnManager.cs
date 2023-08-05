using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private UnitStats unitEnemy;
    [SerializeField] private GameObject enemiesGroup;
    [SerializeField] private float spawnIntervalMin;
    [SerializeField] private float spawnIntervalMax;

    public void StartSpawn()
    {
        StartCoroutine(spawnEnemyRountine());
    }

    private IEnumerator spawnEnemyRountine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            var position = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            Instantiate(unitEnemy.unitPrefab, position.transform.position, Quaternion.identity, enemiesGroup.transform);
        }
    }
}