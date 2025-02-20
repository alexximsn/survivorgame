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
    [NaughtyAttributes.Button]
    public void Animate(int damage)
    {
        damageText.text=damage.ToString();
        animator.Play("Animation");
    }
}
