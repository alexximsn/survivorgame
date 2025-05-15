using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageText : MonoBehaviour
{
    [SerializeField] private Animator animator;//伤害文本的动画
     [SerializeField] private TextMeshPro damageText;
    public void Animate(string damage, bool isCriticalHit)
    {
        damageText.text=damage.ToString();
        damageText.color = isCriticalHit ? new Color(1, 0, 0, 1) : new Color(0, 0, 0, 1);//不暴击黑，暴击红
        animator.Play("Animation");
      
    }
}
