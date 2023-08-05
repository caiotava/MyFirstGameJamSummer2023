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
    [SerializeField] private List<GameObject> uiControlsBuildPhase;

    public void Start()
    {
        StartNewCycle();
    }

    public void StartNewCycle()
    {
        harvestManager.StartHarvest();

        timerDisplay.StartTimer(cycleDuration);
        timerDisplay.OnTimerEnd.AddListener(OnEndOfCycle);

        SetActiveBuildControls(true);
    }

    private void OnEndOfCycle()
    {
        harvestManager.StopHarvest();
        buildManager.StopAllBuildQueues();
        SetActiveBuildControls(false);
    }

    private void SetActiveBuildControls(bool active)
    {
        foreach (var control in uiControlsBuildPhase)
        {
            control.SetActive(active);
        }
    }
}