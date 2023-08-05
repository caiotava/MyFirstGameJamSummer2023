using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleManager : MonoBehaviour
{
    [SerializeField] private TimerDisplay timerDisplay;
    [SerializeField] private HarvestManager harvestManager;
    [SerializeField] private BuildManager buildManager;

    // Start is called before the first frame update
    private void Start()
    {
        timerDisplay.OnTimerEnd.AddListener(OnEndOfCycle);
    }

    private void OnEndOfCycle()
    {
        harvestManager.StopAllCoroutines();
        buildManager.StopAllBuildQueues();
    }
}