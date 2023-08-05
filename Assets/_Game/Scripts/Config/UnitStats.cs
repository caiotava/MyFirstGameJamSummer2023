using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "RTS/UnitStats", order = 1)]
public class UnitStats : ScriptableObject
{
    public uint GoldCost;
    public uint MetalCost;
    public uint WoodCost;
    public float BuildTime;
    public float Health;
    public float Attack;
    public float Defense;
    public float AttackSpeed;
    public uint Supply;
    public Sprite Image;
    public GameObject unitPrefab;
}
