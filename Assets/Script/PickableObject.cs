using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private PickableObjectSO pickableObjectSO;

    private IHuntedObjectsParent pickableObjectParent;

    public PickableObjectSO GetPickableObjectSO()
    {
        return pickableObjectSO;
    }

    public void SetPickableObjectParent(IHuntedObjectsParent pickableObjectParent)
    {
        if (this.pickableObjectParent != null)
        {
            this.pickableObjectParent.ClearPickableObject();
        }
        this.pickableObjectParent = pickableObjectParent;
        if (pickableObjectParent.HasPickableObject())
        {
            Debug.LogError("PickableObjectParent posiada ju¿ obiekt!");
        }
        pickableObjectParent.SetPickableObject(this);
        transform.parent = pickableObjectParent.GetPickableObjectFollowTransform();
        transform.localPosition = Vector3.zero;

    }

    public IHuntedObjectsParent GetKitchenObjectParent()
    {
        return pickableObjectParent;
    }

    public void DestroySelf()
    {
        pickableObjectParent.ClearPickableObject();
        Destroy(gameObject);
    }


    public static PickableObject SpawnPickableObject(PickableObjectSO pickableObjectSO, IHuntedObjectsParent pickableObjectParent)
    {
        Transform pickableObjectTransform = Instantiate(pickableObjectSO.prefab);
        PickableObject pickableObject = pickableObjectTransform.GetComponent<PickableObject>();
        pickableObject.SetPickableObjectParent(pickableObjectParent);
        return pickableObject;
    }
}
