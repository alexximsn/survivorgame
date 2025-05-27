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
    [field: SerializeField]public AnimatorOverrideController AnimOverrideController { get; private set; } //每个角色不同动画
    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float critialChance;
    [SerializeField] private float critialPercent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthRecoverySpeed;
    [SerializeField] private float luck;
    [SerializeField] private float dodge;
 
    public Dictionary<Stat, float> BaseStats//字典
    {
        get
        {
            return new Dictionary<Stat, float>
            {
                { Stat.攻击, attack },
                {Stat.攻击速度,attackSpeed},
                 {Stat.暴击率,critialChance},
                  {Stat.暴击伤害,critialPercent},
                   {Stat.移动速度,moveSpeed},
                    {Stat.生命值,maxHealth},   
                      {Stat.恢复速度,healthRecoverySpeed},
                        {Stat.幸运,luck},
                        {Stat.闪避, dodge},
                      
                   
            };
        }
        private set { }
    }
    public Dictionary<Stat, float> NonNeutralStats
    {
        get
        {
            Dictionary<Stat, float> nonNeutralStats = new Dictionary<Stat, float>();

            foreach (KeyValuePair<Stat, float> kvp in BaseStats)
                if (kvp.Value != 0)
                    nonNeutralStats.Add(kvp.Key, kvp.Value);

            return nonNeutralStats;
        }

        private set { }
    }//包含非0的属性时使用
}
