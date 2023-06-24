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


    private int spawnCount;

    
    [SerializeField] private int spawnCountMax = 4;

    private void Update()
    {
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
            //Debug.Log("Spawned " + randomPickableObjectSO.objectName + " on " + randomTable.gameObject.name);
            spawnCount++;
        }
    }
}
