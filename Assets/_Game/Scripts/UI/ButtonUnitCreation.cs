using UnityEngine;
using UnityEngine.UI;

public class ButtonUnitCreation : MonoBehaviour
{
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private Text GoldText;
    [SerializeField] private Text MetalText;
    [SerializeField] private Text WoodText;
    [SerializeField] private Image UnitImage;
    [SerializeField] private BuildManager buildManager;

    // Start is called before the first frame update
    private void Start()
    {
        GoldText.text = unitStats.GoldCost.ToString();
        MetalText.text = unitStats.MetalCost.ToString();
        WoodText.text = unitStats.WoodCost.ToString();
        UnitImage.sprite = unitStats.Image;
    }

    public void OnButtonClick()
    {
        buildManager.Enqueue(unitStats);
    }
}