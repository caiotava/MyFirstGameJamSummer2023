using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUnitsManager : MonoBehaviour
{
    [SerializeField] private GameObject playerUnitsGroup;
    [SerializeField] private List<UnitStatsComponent> playerSpawnPoints;
    [SerializeField] private uint maxUnitsPerType = 4;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private UnityEvent<List<Unit>> OnFinishSpawn;

    public void SpawnPlayerUnities(ResourceManager resourceManager)
    {
        var unities = resourceManager.TotalSupplyByUnitStats;
        foreach (var unit in playerSpawnPoints)
        {
            if (!unities.ContainsKey(unit.UnitStatsType))
            {
                continue;
            }

            var position = unit.transform;
            var maxUnities = Math.Min(unities[unit.UnitStatsType], maxUnitsPerType);

            for (var x = 0; x < maxUnities; x++)
            {
                var newUnit = Instantiate(
                    unit.UnitStatsType.unitObject,
                    position.transform.position,
                    Quaternion.identity,
                    playerUnitsGroup.transform
                );

                newUnit.InitializeUnitStats(unit.UnitStatsType);
                newUnit.OnUnitKill.AddListener(resourceManager.OnUnitKill);
            }
        }

        OnFinishSpawn.Invoke(GetUnits());
    }

    private List<Unit> GetUnits()
    {
        var units = new List<Unit>();

        foreach (Transform child in playerUnitsGroup.transform)
        {
            units.Add(child.GetComponent<Unit>());
        }

        return units;
    }

    public void ClearAllUnities()
    {
        foreach (Transform child in playerUnitsGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
}