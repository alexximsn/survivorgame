using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponSelectionContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    
    public void Configure(Sprite sprite,string name)
    {
        icon.sprite = sprite;
        nameText.text = name;
    }

}