using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QueueDisplay : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image unitImageInBuilding;
    [SerializeField] private List<Image> unitiesImagesQueued = new();
    
    // Start is called before the first frame update
    void Start()
    {
        progressBar.gameObject.SetActive(false);
        unitImageInBuilding.enabled = false;

        foreach (var image in unitiesImagesQueued)
        {
            image.enabled = false;
        }
    }
    

    public void UpdateControls(UnitStats activeBuild, float buildProgress, List<BuildManager.BuildData> unitiesQueued)
    {
        progressBar.gameObject.SetActive(buildProgress > 0.0f);
        progressBar.value = buildProgress;

        unitImageInBuilding.enabled = activeBuild is not null;
        unitImageInBuilding.sprite = unitImageInBuilding.enabled ? activeBuild.Image : null;
        
        var x = 0;
        for (; x < unitiesQueued.Count && x < unitiesImagesQueued.Count; x++)
        {
            unitiesImagesQueued[x].enabled = true;
            unitiesImagesQueued[x].sprite = unitiesQueued[x].ObjectBeingBuilt.Image;
        }
        
        for (; x < unitiesImagesQueued.Count; x++)
        {
            unitiesImagesQueued[x].enabled = false;
            unitiesImagesQueued[x].sprite = null;
        }
    }
}
