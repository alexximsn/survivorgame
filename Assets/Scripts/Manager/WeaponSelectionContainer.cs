using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class WeaponSelectionContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] public Button Button { get; private set; }
    [SerializeField] private Image[] levelDependentImages;
    public void Configure(Sprite sprite,string name,int level)
    {
        icon.sprite = sprite;
        nameText.text = name;
        Color imageColor
        =ColorHolder.GetColor(level);
        //switch(level){
        //    case 0:
        //        imageColor = Color.red;
        //        break;
        //        case 1:
        //        imageColor = Color.red;
        //        break;
        //    case 2:
        //        imageColor = Color.red;
        //        break;
        //    default:
        //        imageColor = Color.red;
        //        break;

        //}
        foreach (Image image in levelDependentImages)
            image.color = imageColor;
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