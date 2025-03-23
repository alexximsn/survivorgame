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
    [SerializeField] private Sprite statIcon;
    [SerializeField] private Transform statContainersParent;
    [SerializeField] private StatContainer statContainerPrefab;
   // private WeaponDataSO weaponData;
    public void Configure(Sprite sprite,string name,int level,WeaponDataSO weaponData)
    {
        icon.sprite = sprite;
        nameText.text = name;
        Color imageColor
        =ColorHolder.GetColor(level);
       
        foreach (Image image in levelDependentImages)
            image.color = imageColor;
        ConfigureStatContainers(weaponData);
    }

    private void ConfigureStatContainers(WeaponDataSO weaponData)
    {
        foreach(KeyValuePair<Stat,float>kvp in weaponData.BaseStats)
        {
            StatContainer containerInstance = Instantiate(statContainerPrefab,statContainersParent);
            containerInstance.Configure(statIcon, Enums.FormatStatName(kvp.Key), kvp.Value.ToString()) ;
        }
        
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