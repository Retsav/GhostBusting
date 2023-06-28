using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHuntedObjectsParent
{
    public Transform GetPickableObjectFollowTransform();
    public void SetPickableObject(PickableObject pickableObject);
    public PickableObject GetPickableObject();
    public void ClearPickableObject();
    public bool HasPickableObject();
}
