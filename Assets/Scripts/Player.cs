using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{ 
    private PlayerHealth playerHealth;
   private void Awake()
   {
    playerHealth=GetComponent<PlayerHealth>();
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
}
