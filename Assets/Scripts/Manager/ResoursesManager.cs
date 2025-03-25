using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResoursesManager 
{
    const string statIconDataPath = "Data/Stat Icons";
    const string objectDatasPath = "Data/Objects/";
    private static StatIcon[] statIcons;
    public static Sprite GetStatIcon(Stat stat)
    {
        if(statIcons==null )
        {
            StatIconData data = Resources.Load<StatIconData>(statIconDataPath);
            statIcons = data.StatIcons;
        }
        foreach (StatIcon statIcon in statIcons)
            if (stat == statIcon.stat)
                return statIcon.icon;
        Debug.LogError("Ã»ÓÐÍ¼±ê" + stat);
        return null;
    }
    private static ObjectDataSO[] objectDatas;
    public static ObjectDataSO[] Objects
    {
        get {
            if(objectDatas==null)
                
            objectDatas=Resources.LoadAll<ObjectDataSO>(objectDatasPath);
            return objectDatas; }
        private set { }
    }
}
