using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PremiumCurrencyText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public void UpdateText(string currencyString)
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
        text.text = currencyString;
        

    }
}
