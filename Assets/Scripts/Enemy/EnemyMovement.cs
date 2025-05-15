using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
public class EnemyMovement : MonoBehaviour
{
    //怪物需要跟随的玩家和怪物移动速度。
    private Player player;
    [SerializeField] private float moveSpeed;
    public bool IsMoving { get; private set; }
    public bool CanMove { get; private set; } = true;
    public void SetMovementEnabled(bool enable)
    {
        CanMove = enable;
        if (!enable) IsMoving = false;
    }
    void Update()
    {
    }
   
    public void StorePlayer(Player player)
    {
        this.player = player;
    }
    public void FollowPlayer()
    {
        if (!CanMove) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
        IsMoving = true;
    }
}
