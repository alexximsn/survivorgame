using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    [field: SerializeField] public int Currency { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        
    }  
    void Update()
    {
        
    }
    public void AddCurrency(int amount)
    {
        Currency += amount;
    }
}
