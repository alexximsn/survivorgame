using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    //设置最大生命值、当前生命值、生命值条、显示文字
   [SerializeField] private int maxHealth;
   private int health;
   [SerializeField] private Slider healthSlider;
   [SerializeField] private TextMeshProUGUI healthText;
    //开始等于最大生命值，更新UI
    void Start()
    {
        health=maxHealth;
        UpdateUI();
    }
    //1.计算实际受到的伤害（防止生命值为负）2.减少3.更新UI4.检测死亡
      public void TakeDamage(int damage)
    {
        int realDamage=Mathf.Min(damage,health);
        health-=realDamage;
        UpdateUI();
        if(health<=0)
        {
          PassAway();
        }
    }
    private void PassAway()
    {
      Debug.Log("Die");
      SceneManager.LoadScene(0);
    }
    //1.计算当前生命值占最大生命值的比例2.更新生命条3.更新文本
    private void UpdateUI()
    {
        float healthBarValue=(float)health/maxHealth;
        healthSlider.value=healthBarValue;
        healthText.text=health+"/"+maxHealth;
    }
}
