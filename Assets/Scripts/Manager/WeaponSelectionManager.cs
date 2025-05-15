using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;
    [SerializeField] private WeaponDataSO[] starterWeapons;
    [SerializeField] private PlayerWeapons playerWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                if (selectedWeapon == null) return;
                playerWeapons.TryAddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;
            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    private void Configure()
    {
        containersParent.Clear();
        GenerateWeaponContainer(); //直接调用一次生成所有武器
    }

    private void GenerateWeaponContainer()
    {
        //去重并打乱顺序
        List<WeaponDataSO> uniqueWeapons = starterWeapons.Distinct().ToList();
        ShuffleList(uniqueWeapons);
        //确保最多选择3个不重复的武器
        int weaponsToShow = Mathf.Min(3, uniqueWeapons.Count);
        for (int i = 0; i < weaponsToShow; i++)
        {
            WeaponDataSO selectedWeapon = uniqueWeapons[i];
            int level = Random.Range(0, 1); 
            WeaponSelectionContainer container =
                Instantiate(weaponContainerPrefab, containersParent);
            container.Configure(level,selectedWeapon );
            //绑定点击事件
            container.Button.onClick.RemoveAllListeners();
            container.Button.onClick.AddListener(() =>
                WeaponSelectedCallback(container, selectedWeapon, level));
        }
        //武器不足时的警告
        if (uniqueWeapons.Count < 3)
            Debug.LogWarning("武器选择不足");
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            //从末尾开始，随机交换当前位置的元素和一个前面范围内的元素
        }
    }

    private void WeaponSelectedCallback(
        WeaponSelectionContainer containerInstance,
        WeaponDataSO weaponData,
        int level
    )
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = level;

        foreach (WeaponSelectionContainer container in
            containersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == containerInstance) container.Select();
            else container.Deselect();
        }
    }
}
