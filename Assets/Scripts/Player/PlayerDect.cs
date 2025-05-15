using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Player))]
public class PlayerDect : MonoBehaviour
{
    [SerializeField] private Collider2D collectableCollider;
   private void OnTriggerEnter2D(Collider2D collider)
    {
       if(collider.TryGetComponent(out ICollectable collectable))
       {
        if (!collider.IsTouching(collectableCollider))
              return;

            collectable.Collect(GetComponent<Player>());
      }
    }
}
