using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
   [SerializeField] private int maxHealth;
   private int health;
   [SerializeField] private Slider healthSlider;
   [SerializeField] private TextMeshProUGUI healthText;
    void Start()
    {
        health=maxHealth;
        UpdateUI();
    }
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
    private void UpdateUI()
    {
        float healthBarValue=(float)health/maxHealth;
        healthSlider.value=healthBarValue;
        healthText.text=health+"/"+maxHealth;
    }
}
