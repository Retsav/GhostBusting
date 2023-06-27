using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class SpawnRandomPickables :  PickableObject
{
    [SerializeField] private ClearTable[] clearTables;
    [SerializeField] private SpawnableOccultItemsSO spawnableOccultItems;

    private const string PICKABLE_TAG = "Pickable";


    private int spawnCount;

    
    [SerializeField] private int spawnCountMax = 4;

    private void Update()
    {
        spawnCount = GameObject.FindGameObjectsWithTag(PICKABLE_TAG).Length;
        if(spawnCount < spawnCountMax)
        {
            GeneratePickableObjects();
        }
    }



    private void GeneratePickableObjects()
    {
        var randomTable = clearTables.OrderBy(_ => Guid.NewGuid()).First();
        if(!randomTable.HasPickableObject())
        {
            PickableObjectSO randomPickableObjectSO = spawnableOccultItems.pickableObjectsSOList[UnityEngine.Random.Range(0, spawnableOccultItems.pickableObjectsSOList.Count)];
            SpawnPickableObject(randomPickableObjectSO, randomTable);
        }
    }
}
