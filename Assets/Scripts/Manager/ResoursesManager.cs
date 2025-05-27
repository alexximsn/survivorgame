using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//负责加载数据
public static class ResoursesManager 
{
    const string statIconDataPath = "Data/Stat Icons";
    const string objectDatasPath = "Data/Objects/";
    const string weaponDatasPath = "Data/gunsData/";
    const string characterDatasPath = "Data/Character Data/";
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
        Debug.LogError("没有图标" + stat);
        return null;
    }
    private static ObjectDataSO[] objectDatas;
    public static ObjectDataSO[] Objects
    {
        get {
            if(objectDatas==null)
                //如果未空，加载所有数据并缓存
            objectDatas=Resources.LoadAll<ObjectDataSO>(objectDatasPath);
            return objectDatas; }
        private set { }
    }
    public static ObjectDataSO GetRandomObject()//返回一个随机道具
    {
        return Objects[Random.Range(0, Objects.Length)];
    }


    private static WeaponDataSO[] weaponDatas;
    public static WeaponDataSO[] Weapons
    {
        get
        {
            if (weaponDatas == null)

                weaponDatas = Resources.LoadAll<WeaponDataSO>(weaponDatasPath);
            return weaponDatas;
        }
        private set { }
    }
    public static WeaponDataSO GetRandomWeapon()
    {
        return Weapons[Random.Range(0, Weapons.Length)];
    }


    private static CharacterDataSO[] characterDatas;
    public static CharacterDataSO[] Characters
    {
        get
        {
            if (characterDatas == null)

                characterDatas = Resources.LoadAll<CharacterDataSO>(characterDatasPath);
            return characterDatas;
        }
        private set { }
    }
  

}
