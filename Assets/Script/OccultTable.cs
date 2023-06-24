using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultTable : BaseTable
{
    public static OccultTable Instance { get; private set; }
    public event EventHandler<OnPickableTableEventArgs> OnSelectedTableGiven;
    public event EventHandler OnObjectDropped;
    public class OnPickableTableEventArgs : EventArgs
    {
        public PickableObjectSO pickableObjectSO;
    }




    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(PlayerController player)
    {
        if(!HasPickableObject())
        {
            if (player.HasPickableObject())
            {
                if (player.GetPickableObject().IsHunted())
                {
                    player.GetPickableObject().SetPickableObjectParent(this);
                    OnSelectedTableGiven?.Invoke(this, new OnPickableTableEventArgs
                    {
                        pickableObjectSO = GetPickableObject().GetPickableObjectSO()
                    });
                    OnObjectDropped?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
