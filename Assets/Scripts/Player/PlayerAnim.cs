using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private enum playerState { idle,run};
    private Vector2 input;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");

        input.y = Input.GetAxisRaw("Vertical");
        playerAnim();
    }
    void playerAnim()
    {
        playerState states;
        if(Mathf.Abs(input.x)>0|| Mathf.Abs(input.y)>0)
        {
            states = playerState.run;
        }
        else
        {
            states = playerState.idle;
        }
        anim.SetInteger("equal",(int) states);
    }
}
