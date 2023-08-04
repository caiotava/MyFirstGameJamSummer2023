using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildManager : MonoBehaviour
{
    public class BuildData
    {
        public UnitStats ObjectBeingBuilt {  get; private set; }
        public float CurrentBuildProgress { get; private set; }

        public BuildData(UnitStats objectBeingBuilt)
        {
            ObjectBeingBuilt = objectBeingBuilt;
            CurrentBuildProgress = 0.0f;
        }

        public bool Tick(float deltaTime)
        {
            CurrentBuildProgress = Mathf.Clamp01(CurrentBuildProgress + (deltaTime / ObjectBeingBuilt.BuildTime));

            return CurrentBuildProgress >= 1.0f;
        }
    }
    
    [SerializeField] private int QueueSizeLimit = 5;
    [SerializeField] private int GlobalBuildLimit = 100;
    [SerializeField] private ResourceManager resourceManager;

    [SerializeField] UnityEvent<UnitStats> OnBuildQueued = new();
    [SerializeField] UnityEvent<UnitStats> OnBuildStarted = new();
    [SerializeField] UnityEvent<UnitStats, List<BuildData>> OnBuildCompleted = new();
    [SerializeField] UnityEvent<UnitStats, float, List<BuildData>> OnBuildTick = new();
    [SerializeField] UnityEvent<UnitStats> OnNotEnoughResources = new();
    [SerializeField] UnityEvent<UnitStats> OnBuildQueuedLimitSize = new();

    private readonly Queue<BuildData> buildQueue = new();
    private BuildData activeBuildData { get; set; }
    
    public bool Enqueue(UnitStats unitStats)
    {
        if (buildQueue.Count >= QueueSizeLimit)
        {
            OnBuildQueuedLimitSize.Invoke(unitStats);
            return false;
        }

        if (!resourceManager.PayBuildCosts(unitStats))
        {
            OnNotEnoughResources.Invoke(unitStats);
            return false;
        }
        
        buildQueue.Enqueue(new BuildData(unitStats));
        OnBuildQueued.Invoke(unitStats);
        return true;
    }

    public List<BuildData> HeadQueueItems()
    {
        var headQueue = new List<BuildData>(QueueSizeLimit);
        var queue = buildQueue.ToArray();

        for (var x = 0; x < queue.Length && x < QueueSizeLimit; x++)
        {
            headQueue.Add(queue[x]);
        }

        return headQueue;
    }

    public void Update()
    {
        if (buildQueue.Count <= 0 && activeBuildData == null)
        {
            return;
        } 
        
        activeBuildData ??= buildQueue.Dequeue();

        if (activeBuildData == null)
        {
            return;
        }

        if (!activeBuildData.Tick(Time.deltaTime))
        {
            OnBuildTick.Invoke(activeBuildData.ObjectBeingBuilt, activeBuildData.CurrentBuildProgress, HeadQueueItems());
            return;
        }
        
        OnBuildCompleted.Invoke(activeBuildData.ObjectBeingBuilt, HeadQueueItems());
        activeBuildData = null;
    }
}
