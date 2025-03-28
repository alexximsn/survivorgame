using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatsManager))]
public class PlayerObject : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; }
    private PlayerStatsManager playerStatsManager;
    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    void Start()
    {
        foreach (ObjectDataSO objectData in Objects)
            playerStatsManager.AddObject(objectData.BaseStats);
    }

  
    void Update()
    {
        
    }
    public void AddObject(ObjectDataSO objectData)
    {

        Objects.Add(objectData);
        playerStatsManager.AddObject(objectData.BaseStats);
    }
}
