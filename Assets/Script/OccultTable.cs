using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultTable : BaseTable
{
    public override void Interact(PlayerController player)
    {
        if(!HasPickableObject())
        {
            if (player.HasPickableObject())
            {
                if (player.GetPickableObject().IsHunted())
                {
                    player.GetPickableObject().SetPickableObjectParent(this);

                }
            }
        }
    }
}
