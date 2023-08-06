using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private Text textAmount;
    [SerializeField] private uint maxValue = 9999;

    public void SetAmount(uint amount)
    {
        textAmount.text = $"{math.min(amount, maxValue):D4}";
    }

    public void SetAmount(string amount)
    {
        textAmount.text = amount;
    }
}