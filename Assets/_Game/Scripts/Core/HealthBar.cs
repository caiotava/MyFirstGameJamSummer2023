using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    public void OnUnitHit(UnitStats unitStats, float health)
    {
        slider.value = Mathf.Clamp01(health / unitStats.Health);
    }
}
