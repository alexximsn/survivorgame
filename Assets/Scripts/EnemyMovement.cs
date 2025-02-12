using System.Collections;
using System.Collections.Generic;

using System.Threading;

//using System.Diagnostics;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
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
        Vector2 direction =(player.transform.position-transform.position).normalized;
        Vector2 targetPosition=(Vector2)transform.position+direction*moveSpeed*Time.deltaTime;
        transform.position=targetPosition;
    }
    

}
