using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    public UnitStats unitStats { get; private set; }
    public UnityEvent<UnitStats> OnUnitKill;
    public UnityEvent<UnitStats, float> OnUniHit;

    public void InitializeUnitStats(UnitStats stats)
    {
        unitStats = stats;
    }
}
