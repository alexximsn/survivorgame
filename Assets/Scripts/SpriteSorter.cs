using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    //将 Y 位置乘以 10 并强制转换为整数型，这样能够增加较小的 Y 值和较大的 Y 值之间的差异，从而在渲染顺序上创造出更明显的前后关系。
    //例如：当 Y 值增加（物体上移）时，排序值会变得更小（更负），以确保 Y 值较大的物体在渲染时位于 Y 值较小物体的上方。
    //赋值给 spriteRenderer.sortingOrder 来控制精灵的渲染层次，这样使得 Y 轴位置更高的精灵显示在更低层次的精灵之上。
    [SerializeField] private SpriteRenderer spriteRenderer;
    void Start()
    {
        
    }

  
    void Update()
    {
        spriteRenderer.sortingOrder=-(int)(transform.position.y*10);
    }
}
