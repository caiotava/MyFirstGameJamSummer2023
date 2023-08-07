using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsManager : MonoBehaviour
{
    [SerializeField] private GameObject playerUnitsGroup;
    [SerializeField] private List<UnitStatsComponent> playerSpawnPoints;
    [SerializeField] private uint maxUnitsPerType = 4;

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
    }

    public void ClearAllUnities()
    {
        foreach (Transform child in playerUnitsGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
}