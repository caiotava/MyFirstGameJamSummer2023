using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private uint initialGold = 200;
    [SerializeField] private uint initialMetal = 200;
    [SerializeField] private uint initialWood = 200;
    [SerializeField] private uint maxSupply = 100;

    [Header("UI Resources")] 
    [SerializeField] private ResourceDisplay goldDisplay;
    [SerializeField] private ResourceDisplay metalDisplay;
    [SerializeField] private ResourceDisplay woodDisplay;
    [SerializeField] private ResourceDisplay supplyDisplay;
    [SerializeField] private QueueDisplay queueDisplay;
    
    private uint totalGold;
    private uint totalMetal;
    private uint totalWood;
    private Dictionary<UnitStats, uint> totalSupplyByUnitStats = new();
    
    // Start is called before the first frame update
    void Start()
    {
        totalGold = initialGold;
        totalMetal = initialMetal;
        totalWood = initialWood;

        RefreshUIControls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshUIControls()
    {
        goldDisplay.SetAmount(totalGold);
        metalDisplay.SetAmount(totalMetal);
        woodDisplay.SetAmount(totalWood);
        supplyDisplay.SetAmount($"{totalSupply():D3}/{maxSupply:D3}");
    }
    
    public bool PayBuildCosts(UnitStats unitStats) {
        if (!HasEnoughResourceToBuild(unitStats))
        {
            return false;
        }

        totalGold -= unitStats.GoldCost;
        totalMetal -= unitStats.MetalCost;
        totalWood -= unitStats.WoodCost;

        totalSupplyByUnitStats.TryGetValue(unitStats, out var totalUnitsByType);
        totalSupplyByUnitStats[unitStats] = totalUnitsByType + unitStats.Supply;
        
        RefreshUIControls();
        
        return true;
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
        
        foreach (var supply in totalSupplyByUnitStats)
        {
            total += supply.Value;
        }

        return total;
    }
}
