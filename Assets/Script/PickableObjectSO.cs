using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PickableObjectSO : ScriptableObject
{
    public Transform prefab;
    public string objectName;
    public QuickTimeEventsActionListSO qteList;
}
