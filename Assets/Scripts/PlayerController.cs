using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour  
{  
    private Rigidbody2D rig;  
    public float speed;  
    private Vector2 input;  
    private Vector2 mousePos;  

    void Start()  
    {  
        rig = GetComponent<Rigidbody2D>();  
    }  

    // Update is called once per frame  
   private void LateUpdate()
   { 
        // Get input from keyboard  
        input.x = Input.GetAxisRaw("Horizontal");  
        input.y = Input.GetAxisRaw("Vertical");  

        // Update velocity based on input  
        rig.velocity = input.normalized * speed;  

        // Get mouse position in world space  
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  

        // Rotate player based on mouse position  
        if (mousePos.x > transform.position.x)  
        {  
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));  
        }  
        else  
        {  
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));  
        }  
    }  
}  
