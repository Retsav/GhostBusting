using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PickableObjectSO : ScriptableObject
{
    public Transform prefab;
    public Transform exorcisedPrefab;
    public string objectName;
    public QuickTimeEventsActionListSO qteList;
    public float qteTimerMax;
}
