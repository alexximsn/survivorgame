using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChestObjectContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] public Button TakeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }
    [SerializeField] private Image[] levelDependentImages;

    [SerializeField] private Transform statContainersParent;

    // private WeaponDataSO weaponData;
    public void Configure(ObjectDataSO objectData)
    {
        icon.sprite =objectData.Icon;
        nameText.text = name;
        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        nameText.color = imageColor;
        foreach (Image image in levelDependentImages)
            image.color = imageColor;
      
        ConfigureStatContainers(objectData.BaseStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        StatContainersManager.GeneratStatContainers(stats, statContainersParent);

    }

}
