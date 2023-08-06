using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private uint initialGold = 200;
    [SerializeField] private uint initialMetal = 200;
    [SerializeField] private uint initialWood = 200;
    [SerializeField] private uint maxSupply = 100;

    [Header("UI Resources")] [SerializeField]
    private ResourceDisplay goldDisplay;

    [SerializeField] private ResourceDisplay metalDisplay;
    [SerializeField] private ResourceDisplay woodDisplay;
    [SerializeField] private ResourceDisplay supplyDisplay;
    [SerializeField] private QueueDisplay queueDisplay;

    private uint totalGold;
    private uint totalMetal;
    private uint totalWood;
    public Dictionary<UnitStats, uint> TotalSupplyByUnitStats { get; } = new();

    // Start is called before the first frame update
    private void Start()
    {
        totalGold = initialGold;
        totalMetal = initialMetal;
        totalWood = initialWood;

        RefreshUIControls();
    }

    public void RefreshUIControls()
    {
        goldDisplay.SetAmount(totalGold);
        metalDisplay.SetAmount(totalMetal);
        woodDisplay.SetAmount(totalWood);
        supplyDisplay.SetAmount($"{totalSupply():D3}/{maxSupply:D3}");
    }

    public bool PayBuildCosts(UnitStats unitStats)
    {
        if (!HasEnoughResourceToBuild(unitStats))
        {
            return false;
        }

        totalGold -= unitStats.GoldCost;
        totalMetal -= unitStats.MetalCost;
        totalWood -= unitStats.WoodCost;

        TotalSupplyByUnitStats.TryGetValue(unitStats, out var totalUnitsByType);
        TotalSupplyByUnitStats[unitStats] = totalUnitsByType + 1;

        RefreshUIControls();

        return true;
    }

    public void RefundBuildCosts(UnitStats unitStats)
    {
        totalGold += unitStats.GoldCost;
        totalMetal += unitStats.MetalCost;
        totalWood += unitStats.WoodCost;

        TotalSupplyByUnitStats.TryGetValue(unitStats, out var totalUnitsByType);
        TotalSupplyByUnitStats[unitStats] = totalUnitsByType - 1;

        RefreshUIControls();
    }

    public bool HasEnoughResourceToBuild(UnitStats unitStats)
    {
        return unitStats.GoldCost <= totalGold &&
               unitStats.MetalCost <= totalMetal &&
               unitStats.WoodCost <= totalWood &&
               totalSupply() + unitStats.Supply <= maxSupply;
    }

    public void OnBuildQueued(UnitStats unitStats)
    {
    }

    public void OnBuildCancelled(UnitStats unitStats, List<BuildManager.BuildData> queuedUnits)
    {
        if (unitStats is not null)
        {
            RefundBuildCosts(unitStats);
        }

        queueDisplay.UpdateControls(unitStats, 0, queuedUnits);
    }

    public void OnBuildCompleted(UnitStats unitStats, List<BuildManager.BuildData> queuedUnits)
    {
        if (queuedUnits.Count == 0)
        {
            queueDisplay.UpdateControls(null, 0, queuedUnits);
        }
    }

    public void OnBuildTick(UnitStats unitStats, float percentage, List<BuildManager.BuildData> queuedUnits)
    {
        queueDisplay.UpdateControls(unitStats, percentage, queuedUnits);
    }

    public void OnBuildQueuedLimitSize(UnitStats unitStats)
    {
        Debug.Log($"Queue limit size {unitStats.Image.name}");
    }

    public void OnNotEnoughResources(UnitStats unitStats)
    {
        Debug.Log($"not enough resources {unitStats.Image.name}");
    }

    private uint totalSupply()
    {
        uint total = 0;

        foreach (var supply in TotalSupplyByUnitStats)
        {
            total += supply.Key.Supply * supply.Value;
        }

        return total;
    }

    public void AddGold(uint amount)
    {
        if (amount <= 0)
        {
            return;
        }

        totalGold += amount;
        RefreshUIControls();
    }

    public void AddMetal(uint amount)
    {
        if (amount <= 0)
        {
            return;
        }

        totalMetal += amount;
        RefreshUIControls();
    }

    public void AddWood(uint amount)
    {
        if (amount <= 0)
        {
            return;
        }

        totalWood += amount;
        RefreshUIControls();
    }
}