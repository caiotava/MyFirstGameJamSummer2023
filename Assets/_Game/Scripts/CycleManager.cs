using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleManager : MonoBehaviour
{
    [SerializeField] private TimerDisplay timerDisplay;
    [SerializeField] private HarvestManager harvestManager;
    [SerializeField] private BuildManager buildManager;
    [SerializeField] private float cycleDuration;

    public void Start()
    {
        StartNewCycle();
    }

    public void StartNewCycle()
    {
        harvestManager.StartHarvest();

        timerDisplay.StartTimer(cycleDuration);
        timerDisplay.OnTimerEnd.AddListener(OnEndOfCycle);
    }

    private void OnEndOfCycle()
    {
        harvestManager.StopHarvest();
        buildManager.StopAllBuildQueues();
    }
}