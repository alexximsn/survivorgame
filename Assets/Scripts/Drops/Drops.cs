using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Drops : DroppableCurrency
{
    public static Action<Drops> onCollected;
    protected override void Collected()
    {
        onCollected?.Invoke(this);
        ObjectPool.Instance.PushObject(gameObject);
    }

}
