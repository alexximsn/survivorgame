using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
public class WeaponSelectionContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] public Button Button { get; private set; }
    [SerializeField] private Image[] levelDependentImages;
   
    [SerializeField] private Transform statContainersParent;
   
   // private WeaponDataSO weaponData;
    public void Configure(int level,WeaponDataSO weaponData)
    {
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name+$"\nlv{level+1}";
        Color imageColor =ColorHolder.GetColor(level);
        nameText.color = imageColor;
        foreach (Image image in levelDependentImages)
            image.color = imageColor;
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainersManager.GeneratStatContainers(calculatedStats, statContainersParent);
        
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, .3f).setEase(LeanTweenType.easeInOutSine);
    }
    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one ,.3f);
    }

}