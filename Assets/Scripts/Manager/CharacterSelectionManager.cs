using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Tabsil.SaverManager;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
 
    [SerializeField] private Transform characterButtonsParent;//ѡ��ĸ�����
    [SerializeField] private CharacterButton characterButtonPrefab;
    [SerializeField] private Image centerCharacterImage;//��ɫͼƬ����ͷ�գ�
    [SerializeField] private CharacterInfoPanel characterInfo;//�����


    private CharacterDataSO[] characterDatas;
    private List<bool> unlockedStates = new List<bool>();
    private const string unlockedStatesKey = "unlockedStatesKey";
    private const string lastSelectedCharacterKey = "lastSelectedCharacterKey";

    private int lastSelectedCharacterIndex;//���һ��ѡ��Ķ���
    private int selectedCharacterIndex;


    [Header(" Actions ")]
    public static Action<CharacterDataSO> onCharacterSelected;//��̬�¼�����ɫѡ��ʱ��������ʾ����

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        characterInfo.Button.onClick.RemoveAllListeners();
        characterInfo.Button.onClick.AddListener(PurchaseSelectedCharacter);
        CharacterSelectedCallback(lastSelectedCharacterIndex);//�����ϴ�ѡ��Ľ�ɫ
    }


    private void Initialize()
    {
        for (int i = 0; i < characterDatas.Length; i++)
            CreateCharacterButton(i);//������ɫѡ��
    }

    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatas[index];
        CharacterButton characterButtonInstance = Instantiate(characterButtonPrefab, characterButtonsParent);
        characterButtonInstance.Configure(characterData.Sprite, unlockedStates[index]);
        characterButtonInstance.Button.onClick.RemoveAllListeners();//���ð�ť��ͼƬ�ͽ���״̬��Ϊ��ť��ӵ���¼�������
        characterButtonInstance.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    private void CharacterSelectedCallback(int index)
    {
        selectedCharacterIndex = index;//��ǰѡ�н�ɫ����
        CharacterDataSO characterData = characterDatas[index];
        if (unlockedStates[index])//����״̬
        {
            lastSelectedCharacterIndex = index;
            characterInfo.Button.interactable = false;//����ť����
            Save();
            onCharacterSelected?.Invoke(characterData);//����ѡ���¼�
        }
        else
        {
            characterInfo.Button.interactable =
                CurrencyManager.instance.HasEnoughPremiumCurrency(characterData.PurchasePrice);//δ�ܽ�����Ǯ�Ƿ�
        }
        centerCharacterImage.sprite = characterData.Sprite;
        characterInfo.Configure(characterData, unlockedStates[index]);
    }

    private void PurchaseSelectedCharacter()
    {
        int price = characterDatas[selectedCharacterIndex].PurchasePrice;
        CurrencyManager.instance.UsePremiumCurrency(price);//��Ǯ

        unlockedStates[selectedCharacterIndex] = true;//����
        characterButtonsParent.GetChild(selectedCharacterIndex).GetComponent<CharacterButton>().Unlock();
        CharacterSelectedCallback(selectedCharacterIndex);//ѡ��
        Save();//����
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
