using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName ="Character Data",menuName ="Scriptable Object/New Character Data",order =0)]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }
    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float critialChance;
    [SerializeField] private float critialPercent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float range;
    [SerializeField] private float healthRecoverySpeed;
    [SerializeField] private float armor;
    [SerializeField] private float luck;
    [SerializeField] private float dodge;
    [SerializeField] private float lifeSteal;
    [SerializeField] private float spreadCount;
    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            return new Dictionary<Stat, float>
            {
                { Stat.Attack, attack },
                {Stat.AttackSpeed,attackSpeed},
                 {Stat.CritialChange,critialChance},
                  {Stat.CritialPercent,critialPercent},
                   {Stat.Movespeed,moveSpeed},
                    {Stat.MaxHealth,maxHealth},
                     {Stat.Range,range},
                      {Stat.HealthRecoverySpeed,healthRecoverySpeed},
                       {Stat.Armor,armor},
                        {Stat.Lucky,luck},
                        {Stat.Dodge, dodge},
                        {Stat.LifeSteal,lifeSteal},
                        {Stat.SpreadCount,spreadCount},
            };
        }
        private set { }
    }
}
