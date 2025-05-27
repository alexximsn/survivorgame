using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Tabsil.SaverManager;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
 
    [SerializeField] private Transform characterButtonsParent;//选项的父对象
    [SerializeField] private CharacterButton characterButtonPrefab;
    [SerializeField] private Image centerCharacterImage;//角色图片（大头照）
    [SerializeField] private CharacterInfoPanel characterInfo;//详情框


    private CharacterDataSO[] characterDatas;
    private List<bool> unlockedStates = new List<bool>();
    private const string unlockedStatesKey = "unlockedStatesKey";
    private const string lastSelectedCharacterKey = "lastSelectedCharacterKey";

    private int lastSelectedCharacterIndex;//最后一个选择的对象
    private int selectedCharacterIndex;


    [Header(" Actions ")]
    public static Action<CharacterDataSO> onCharacterSelected;//静态事件，角色选中时触发，显示详情

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        characterInfo.Button.onClick.RemoveAllListeners();
        characterInfo.Button.onClick.AddListener(PurchaseSelectedCharacter);
        CharacterSelectedCallback(lastSelectedCharacterIndex);//加载上次选择的角色
    }


    private void Initialize()
    {
        for (int i = 0; i < characterDatas.Length; i++)
            CreateCharacterButton(i);//创建角色选项
    }

    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatas[index];
        CharacterButton characterButtonInstance = Instantiate(characterButtonPrefab, characterButtonsParent);
        characterButtonInstance.Configure(characterData.Sprite, unlockedStates[index]);
        characterButtonInstance.Button.onClick.RemoveAllListeners();//配置按钮的图片和解锁状态，为按钮添加点击事件监听器
        characterButtonInstance.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    private void CharacterSelectedCallback(int index)
    {
        selectedCharacterIndex = index;//当前选中角色索引
        CharacterDataSO characterData = characterDatas[index];
        if (unlockedStates[index])//解锁状态
        {
            lastSelectedCharacterIndex = index;
            characterInfo.Button.interactable = false;//购买按钮隐藏
            Save();
            onCharacterSelected?.Invoke(characterData);//触发选择事件
        }
        else
        {
            characterInfo.Button.interactable =
                CurrencyManager.instance.HasEnoughPremiumCurrency(characterData.PurchasePrice);//未能解锁，钱是否够
        }
        centerCharacterImage.sprite = characterData.Sprite;
        characterInfo.Configure(characterData, unlockedStates[index]);
    }

    private void PurchaseSelectedCharacter()
    {
        int price = characterDatas[selectedCharacterIndex].PurchasePrice;
        CurrencyManager.instance.UsePremiumCurrency(price);//花钱

        unlockedStates[selectedCharacterIndex] = true;//解锁
        characterButtonsParent.GetChild(selectedCharacterIndex).GetComponent<CharacterButton>().Unlock();
        CharacterSelectedCallback(selectedCharacterIndex);//选中
        Save();//保存
    }

    public void Load()
    {
        characterDatas = ResoursesManager.Characters;

        for (int i = 0; i < characterDatas.Length; i++)
            unlockedStates.Add(i == 0);

        if (SaverManager.TryLoad(this, unlockedStatesKey, out object unlockedStatesObject))
            unlockedStates = (List<bool>)unlockedStatesObject;

        if (SaverManager.TryLoad(this, lastSelectedCharacterKey, out object lastSelectedCharacterObject))
            lastSelectedCharacterIndex = (int)lastSelectedCharacterObject;
        Initialize();
    }

    public void Save()
    {
        SaverManager.Save(this, unlockedStatesKey, unlockedStates);
        SaverManager.Save(this, lastSelectedCharacterKey, lastSelectedCharacterIndex);
    }

}
