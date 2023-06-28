using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTable : BaseTable
{
    //[SerializeField] private PickableObjectSO pickableObjectSO;


    public override void Interact(PlayerController player)
    {
        if (!HasPickableObject())
        {
            if (player.HasPickableObject())
            {
                player.GetPickableObject().SetPickableObjectParent(this);
            }
        }
        else
        {
            if (!player.HasPickableObject())
            {
                GetPickableObject().SetPickableObjectParent(player);
            } else
            {
                //Table i player maja po przedmiocie
            }
        }
    }
}
