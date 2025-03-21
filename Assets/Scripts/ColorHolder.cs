using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    public static ColorHolder instance;
    [SerializeField] private PaletteSO palette;
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public static Color GetColor(int level)
    {
        level = Mathf.Clamp(level, 0, instance.palette.LevelColor.Length);
        return instance.palette.LevelColor[level];
    }
    public static Color GetOutlineColor(int level)
    {
        level = Mathf.Clamp(level, 0, instance.palette.LevelOutlineColors.Length);
        return instance.palette.LevelOutlineColors[level];
    }
}
