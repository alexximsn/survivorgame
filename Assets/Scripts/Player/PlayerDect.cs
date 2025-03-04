using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Player))]
public class PlayerDect : MonoBehaviour
{
    [SerializeField] private Collider2D collectableCollider;
    //private void FixedUpdate()
    //{
    //    Collider2D[] cherriesColliders = Physics2D.OverlapCircleAll(
    //        (Vector2)transform.position + playerCollider.offset,
    //        playerCollider.radius);
    //    foreach(Collider2D collider in cherriesColliders)
    //    {
    //        if (collider.TryGetComponent(Drops drop))
    //        {
    //            Destroy(drop);
    //        }

    //    }
   // }
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
