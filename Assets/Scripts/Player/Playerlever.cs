using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class Playerlever : MonoBehaviour
{
    private int requiredXp;
    private int currentXp;
    private int level;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        Drops.onCollected += DropsCollectedCallback;
    }
    private void OnDestroy()
    {
        Drops.onCollected -= DropsCollectedCallback;
    }
    void Start()
    {
        UpdateRequiredXp();
        UpdateVisuals();
        
    }

    private void UpdateRequiredXp()
    {
        requiredXp = (level + 1) * 5;
    }
    private void UpdateVisuals()
    {
        xpBar.value = (float)currentXp / requiredXp;
        levelText.text = "Level" + (level + 1);
    }
    private void DropsCollectedCallback(Drops drop)
    {
        currentXp++;
        if (currentXp >= requiredXp)
            LevelUp();
        UpdateVisuals();
    }
    private void LevelUp()
    {
        level++;
        currentXp = 0;
        UpdateRequiredXp();
    }

}
