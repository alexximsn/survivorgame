using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Weapon Data", 
    menuName = "Scriptable Object/New Weapon Data", order = 0)]
public class WeaponDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }
    [field: SerializeField] public weapons Prefab { get; private set; }
    [field: SerializeField] public int RecyclePrice{ get; private set; }
    [field:SerializeField] public AudioClip AttackSound { get; private set; }
    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float critialChance;
    [SerializeField] private float critialPercent;
 

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            return new Dictionary<Stat, float>
            {
                { Stat.����, attack },
                {Stat.�����ٶ�,attackSpeed},
                 {Stat.������,critialChance},
                  {Stat.�����˺�,critialPercent},             
            };
        }
        private set { }
    }
    public float GetStatValue(Stat stat)
    {
        foreach (KeyValuePair<Stat, float> kvp in BaseStats)
            if (kvp.Key == stat)
                return kvp.Value;
        Debug.LogError("û�ҵ���ֵ");
        return 0;
    }
}
