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
        GenerateWeaponContainer(); //ֱ�ӵ���һ��������������
    }

    private void GenerateWeaponContainer()
    {
        //ȥ�ز�����˳��
        List<WeaponDataSO> uniqueWeapons = starterWeapons.Distinct().ToList();
        ShuffleList(uniqueWeapons);
        //ȷ�����ѡ��3�����ظ�������
        int weaponsToShow = Mathf.Min(3, uniqueWeapons.Count);
        for (int i = 0; i < weaponsToShow; i++)
        {
            WeaponDataSO selectedWeapon = uniqueWeapons[i];
            int level = Random.Range(0, 1); 
            WeaponSelectionContainer container =
                Instantiate(weaponContainerPrefab, containersParent);
            container.Configure(level,selectedWeapon );
            //�󶨵���¼�
            container.Button.onClick.RemoveAllListeners();
            container.Button.onClick.AddListener(() =>
                WeaponSelectedCallback(container, selectedWeapon, level));
        }
        //��������ʱ�ľ���
        if (uniqueWeapons.Count < 3)
            Debug.LogWarning("����ѡ����");
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            //��ĩβ��ʼ�����������ǰλ�õ�Ԫ�غ�һ��ǰ�淶Χ�ڵ�Ԫ��
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
