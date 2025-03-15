using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerHealth),typeof(Playerlever))]
public class Player : MonoBehaviour
{
    public static Player instance;
    private PlayerHealth playerHealth;
    [SerializeField] private CircleCollider2D collider;
    private Playerlever playerLevel;
   private void Awake()
   {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

     playerHealth=GetComponent<PlayerHealth>();
        playerLevel = GetComponent<Playerlever>();
   }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
      playerHealth.TakeDamage(damage);
    }
    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + collider.offset;//返回玩家碰撞的中心，以确定射击位置
    }
    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
}
