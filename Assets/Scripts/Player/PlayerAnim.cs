using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private Vector2 input;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        bool isMoving = Mathf.Abs(input.x) > 0 || Mathf.Abs(input.y) > 0;
        anim.SetBool("IsMoving", isMoving); // 
    }
}
