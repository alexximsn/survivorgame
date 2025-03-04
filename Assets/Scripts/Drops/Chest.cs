using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chest : MonoBehaviour,ICollectable
{
    public static Action onCollected;
   public void Collect(Player player)
    {
        onCollected?.Invoke();
        Destroy(gameObject);
    }
}
