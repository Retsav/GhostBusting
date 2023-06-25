using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultTable : BaseTable
{
    public static OccultTable Instance { get; private set; }
    public event EventHandler<OnPickableTableEventArgs> OnSelectedTableGiven;
    public event EventHandler<OnCorrectButtonPressedEventArgs> OnCorrectButtonPressed;
    public event EventHandler OnObjectDropped;
    public event EventHandler OnObjectPicked;
    public event EventHandler OnFinishedExorcising;
    public class OnPickableTableEventArgs : EventArgs
    {
        public PickableObjectSO pickableObjectSO;
    }

    public class OnCorrectButtonPressedEventArgs : EventArgs
    {
        public QTE_SO qteSO;
    }

    [SerializeField] private List<QTE_SO> qteSOList;

    public enum State
    {
        Idle,
        Exorcising,
    }

    [SerializeField] private State state;

    private void Awake()
    {
        Instance = this;
        state = State.Idle;
    }

    private void Update()
    {
        if(state == State.Exorcising)
        {

            foreach(QTE_SO qteSO in GetPickableObject().GetPickableObjectQTEList().quickTimeEventActionList)
            {
                if(Input.GetKeyDown(qteSO.keyToPress) && qteSOList.Count > 0)
                {
                    qteSOList.Remove(qteSO);
                    OnCorrectButtonPressed?.Invoke(this, new OnCorrectButtonPressedEventArgs
                    {
                        qteSO = qteSO
                    });
                    
                } else if (qteSOList.Count == 0)
                {
                    state = State.Idle;
                    OnFinishedExorcising?.Invoke(this, EventArgs.Empty);
                }
            }
        }
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
                    for(int i = 0; i < GetPickableObject().GetPickableObjectQTEList().quickTimeEventActionList.Count; i++)
                    {
                        qteSOList.Add(GetPickableObject().GetPickableObjectQTEList().quickTimeEventActionList[i]);
                    }
                }
            } 
        } else {
            GetPickableObject().SetPickableObjectParent(player);
            OnObjectPicked?.Invoke(this, EventArgs.Empty);
            qteSOList.Clear();
        }
    }

    public override void InteractAlternate(PlayerController player)
    {
        if(HasPickableObject()) 
        { 
            if(GetPickableObject().IsHunted())
            {
                state = State.Exorcising;
            }
        }
    }
}
