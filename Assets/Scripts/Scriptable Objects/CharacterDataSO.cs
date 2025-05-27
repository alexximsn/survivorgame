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
    [field: SerializeField]public AnimatorOverrideController AnimOverrideController { get; private set; } //ÿ����ɫ��ͬ����
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
 
    public Dictionary<Stat, float> BaseStats//�ֵ�
    {
        get
        {
            return new Dictionary<Stat, float>
            {
                { Stat.����, attack },
                {Stat.�����ٶ�,attackSpeed},
                 {Stat.������,critialChance},
                  {Stat.�����˺�,critialPercent},
                   {Stat.�ƶ��ٶ�,moveSpeed},
                    {Stat.����ֵ,maxHealth},   
                      {Stat.�ָ��ٶ�,healthRecoverySpeed},
                        {Stat.����,luck},
                        {Stat.����, dodge},
                      
                   
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
    }//������0������ʱʹ��
}
