using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageText : MonoBehaviour
{
    [SerializeField] private Animator animator;
     [SerializeField] private TextMeshPro damageText;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void Animate(int damage, bool isCriticalHit)
    {
        damageText.text=damage.ToString();
        damageText.color = isCriticalHit ? new Color(1, 0, 0, 1) : new Color(0, 0, 0, 1);
        animator.Play("Animation");
       // Debug.Log($"Damage: {damage}, IsCriticalHit: {isCriticalHit}, Color: {damageText.color}");
    }
}
