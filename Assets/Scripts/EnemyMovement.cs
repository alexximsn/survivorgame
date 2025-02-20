using System.Collections;
using System.Collections.Generic;

using System.Threading;

//using System.Diagnostics;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //怪物需要跟随的玩家和怪物移动速度。
    private Player player;
    [SerializeField] private float moveSpeed;
    
    void Update()
    {
       
        if(player!=null)
          FollowPlayer();  
    }
    public void StorePlayer(Player player)
    {
        this.player = player;
    }
    private void FollowPlayer()
    {
        //1.计算方向2.计算目标位置3.更新位置
        Vector2 direction =(player.transform.position-transform.position).normalized;
        Vector2 targetPosition=(Vector2)transform.position+direction*moveSpeed*Time.deltaTime;
        transform.position=targetPosition;
    }
    

}
