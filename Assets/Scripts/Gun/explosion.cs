using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime>=1)
        {
            Destroy(gameObject);
        }
        
    }
}
