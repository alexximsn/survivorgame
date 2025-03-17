using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour ,IPlayerStatsDependency 
{  
    //刚体、走路速度、玩家输出的方向、鼠标在游戏中的位置
    private Rigidbody2D rig;
    [SerializeField] private float baseMoveSpeed;
     private float MoveSpeed;  
    private Vector2 input;  
    private Vector2 mousePos;
    
    void Start()  
    {  
        rig = GetComponent<Rigidbody2D>();
        
    }  

   private void LateUpdate()
   { 
        input.x = Input.GetAxisRaw("Horizontal");  
        
        input.y = Input.GetAxisRaw("Vertical");
        //获得原始值
        rig.velocity = input.normalized * MoveSpeed;  
        //设置速度
      
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  
        //获取鼠标位置，如果鼠标在左，人物朝左；反之。
        if (mousePos.x > transform.position.x)  
        {  
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));  
        }  
        else  
        {  
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));  
        }  
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatValue(Stat.Movespeed) / 100;
        MoveSpeed = baseMoveSpeed * (1 + moveSpeedPercent);
    }
}  
