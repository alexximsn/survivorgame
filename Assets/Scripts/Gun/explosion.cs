using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator animator;
    private AnimatorStateInfo info;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }
   
    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }

    }
}
