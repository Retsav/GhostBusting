using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTable : MonoBehaviour, IHuntedObjectsParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    [SerializeField] private Transform tableTopPoint;
    private PickableObject pickableObject;


    public virtual void Interact(PlayerController player)
    {
        Debug.LogError("BaseTable.Interact()");
    }

    public virtual void InteractAlternate(PlayerController player)
    {
        // Debug.LogError("BaseCounter.InteractAlternate()");
    }

    public void ClearPickableObject()
    {
        pickableObject = null;
    }

    public PickableObject GetPickableObject()
    {
        return pickableObject;
    }

    public Transform GetPickableObjectFollowTransform()
    {
        return tableTopPoint;
    }

    public bool HasPickableObject()
    {
        return pickableObject != null;
    }

    public void SetPickableObject(PickableObject pickableObject)
    {
        this.pickableObject = pickableObject;
        if (pickableObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
}
