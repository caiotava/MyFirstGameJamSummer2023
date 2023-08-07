using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public UnitStats unitStats { get; private set; }
    public UnityEvent<UnitStats> OnUnitKill;

    public void InitializeUnitStats(UnitStats stats)
    {
        unitStats = stats;
    }
}
