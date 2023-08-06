using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private int QueueSizeLimit = 5;
    [SerializeField] private int GlobalBuildLimit = 100;
    [SerializeField] private ResourceManager resourceManager;

    [SerializeField] private UnityEvent<UnitStats> OnBuildQueued = new();
    [SerializeField] private UnityEvent<UnitStats> OnBuildStarted = new();
    [SerializeField] private UnityEvent<UnitStats, List<BuildData>> OnBuildCompleted = new();
    [SerializeField] private UnityEvent<UnitStats, float, List<BuildData>> OnBuildTick = new();
    [SerializeField] private UnityEvent<UnitStats> OnNotEnoughResources = new();
    [SerializeField] private UnityEvent<UnitStats> OnBuildQueuedLimitSize = new();
    [SerializeField] private UnityEvent<UnitStats, List<BuildData>> OnBuildCancelled = new();

    private readonly Queue<BuildData> buildQueue = new();
    private BuildData activeBuildData { get; set; }

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
            OnBuildTick.Invoke(
                activeBuildData.ObjectBeingBuilt,
                activeBuildData.CurrentBuildProgress,
                HeadQueueItems()
            );
            return;
        }

        OnBuildCompleted.Invoke(activeBuildData.ObjectBeingBuilt, HeadQueueItems());
        activeBuildData = null;
    }

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

    public void StopAllBuildQueues()
    {
        while (buildQueue.Count > 0)
        {
            OnBuildCancelled.Invoke(buildQueue.Dequeue().ObjectBeingBuilt, HeadQueueItems());
        }

        if (activeBuildData is not null)
        {
            resourceManager.RefundBuildCosts(activeBuildData.ObjectBeingBuilt);
            activeBuildData = null;
        }

        OnBuildCancelled.Invoke(null, HeadQueueItems());
    }

    public class BuildData
    {
        public BuildData(UnitStats objectBeingBuilt)
        {
            ObjectBeingBuilt = objectBeingBuilt;
            CurrentBuildProgress = 0.0f;
        }

        public UnitStats ObjectBeingBuilt { get; }
        public float CurrentBuildProgress { get; private set; }

        public bool Tick(float deltaTime)
        {
            CurrentBuildProgress = Mathf.Clamp01(CurrentBuildProgress + deltaTime / ObjectBeingBuilt.BuildTime);

            return CurrentBuildProgress >= 1.0f;
        }
    }
}