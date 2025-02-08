using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SpriteRenderer spriteRenderer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder=-(int)(transform.position.y*10);
    }
}
