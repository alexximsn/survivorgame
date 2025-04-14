using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject priceContainer;
    [SerializeField] private Transform statsParent;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(CharacterDataSO characterData, bool unlocked)
    {
        nameText.text = characterData.Name;
        priceText.text = characterData.PurchasePrice.ToString();

        priceContainer.SetActive(!unlocked);

        StatContainersManager.GeneratStatContainers(characterData.NonNeutralStats, statsParent);
    }
}
