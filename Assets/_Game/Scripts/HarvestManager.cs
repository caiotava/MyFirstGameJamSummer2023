using System;
using System.Collections;
using UnityEngine;

public class HarvestManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private float speedHarvestGold = 2;
    [SerializeField] private float speedHarvestMetal = 1.8f;
    [SerializeField] private float speedHarvestWood = 1.7f;

    [SerializeField] private uint collectGold = 10;
    [SerializeField] private uint collectMetal = 10;
    [SerializeField] private uint collectWood = 10;

    private static IEnumerator harvestRoutine(float speedHarvest, uint amount, Action<uint> setResourceCallback)
    {
        var waitTime = new WaitForSeconds(speedHarvest);
        while (true)
        {
            yield return waitTime;

            setResourceCallback(amount);
        }
    }

    public void StartHarvest()
    {
        StartCoroutine(harvestRoutine(speedHarvestGold, collectGold, resourceManager.AddGold));
        StartCoroutine(harvestRoutine(speedHarvestMetal, collectMetal, resourceManager.AddMetal));
        StartCoroutine(harvestRoutine(speedHarvestWood, collectWood, resourceManager.AddWood));
    }

    public void StopHarvest()
    {
        StopAllCoroutines();
    }
}