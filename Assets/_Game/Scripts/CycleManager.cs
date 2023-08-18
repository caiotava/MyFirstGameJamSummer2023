using System.Collections.Generic;
using UnityEngine;

public class CycleManager : MonoBehaviour
{
    [SerializeField] private float cycleDurationBuild;
    [SerializeField] private float cycleDurationBattle;
    [SerializeField] private List<GameObject> uiControlsBuildPhase;

    [Header("Managers")] [SerializeField] private TimerDisplay timerDisplay;
    [SerializeField] private HarvestManager harvestManager;
    [SerializeField] private BuildManager buildManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PlayerUnitsManager playerUnitsManager;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameControl gameControl;

    private int totalUnitsPlayer;
    private int totalUnitsEnemy;

    public void Start()
    {
        spawnManager.OnUnitSpawn.AddListener(OnEnemySpawn);
        StartNewCycleBuild();
    }

    public void StartNewCycleBuild()
    {
        timerDisplay.OnTimerEnd.RemoveAllListeners();
        timerDisplay.StartTimer(cycleDurationBuild);
        timerDisplay.OnTimerEnd.AddListener(OnEndOfCycleBuild);

        harvestManager.StartHarvest();

        SetActiveBuildControls(true);
        gameControl.enabled = true;
    }

    public void StartNewCycleBattle()
    {
        timerDisplay.OnTimerEnd.RemoveAllListeners();
        timerDisplay.StartTimer(cycleDurationBattle);
        timerDisplay.OnTimerEnd.AddListener(OnEndOfCycleBattle);

        SetActiveBuildControls(false);
        gameControl.enabled = true;
        playerUnitsManager.SpawnPlayerUnities(resourceManager);
        spawnManager.StartSpawn();
    }

    private void OnEndOfCycleBuild()
    {
        harvestManager.StopHarvest();
        buildManager.StopAllBuildQueues();

        gameControl.enabled = false;
        StartNewCycleBattle();
    }

    private void OnEndOfCycleBattle()
    {
        playerUnitsManager.ClearAllUnities();
        spawnManager.ClearAllUnities();
        spawnManager.StopAllCoroutines();

        gameControl.enabled = false;
        StartNewCycleBuild();
    }

    public void OnFinishToSpawnPlayerUnits(List<Unit> units)
    {
        totalUnitsPlayer = units.Count;
        foreach (var unit in units)
        {
            unit.OnUnitKill.AddListener(onPlayerUnitKill);
        }
    }

    public void OnStartToSpawnEnemies(int total)
    {
        totalUnitsEnemy = total;
    }

    public void OnEnemySpawn(Unit unit)
    {
        unit.OnUnitKill.AddListener(OnEnemyUnitKill);
    }

    public void OnEnemyUnitKill(UnitStats unitStats)
    {
        totalUnitsEnemy--;
        if (totalUnitsEnemy <= 0)
        {
            OnEndOfCycleBattle();
        }
    }

    private void onPlayerUnitKill(UnitStats unitStats)
    {
        totalUnitsPlayer--;

        if (totalUnitsPlayer <= 0)
        {
            OnEndOfCycleBattle();
        }
    }

    private void SetActiveBuildControls(bool active)
    {
        foreach (var control in uiControlsBuildPhase)
        {
            control.SetActive(active);
        }
    }
}