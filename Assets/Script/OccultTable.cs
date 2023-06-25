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

    private int keyClickedIndex;
    [SerializeField] private GameInput gameInput;
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
            KeyCode buttonPressed = gameInput.GetKeyPressed();
            if (Input.GetKeyDown(qteSOList[keyClickedIndex].keyToPress))
                {
                keyClickedIndex++;
                if (keyClickedIndex == qteSOList.Count)
                {
                    state = State.Idle;
                    OnFinishedExorcising?.Invoke(this, EventArgs.Empty);
                    keyClickedIndex = 0;
                    qteSOList.Clear();
                } else
                {
                    OnCorrectButtonPressed?.Invoke(this, new OnCorrectButtonPressedEventArgs
                    {
                        qteSO = qteSOList[keyClickedIndex]
                    });
                }                    
            } else if (Input.anyKeyDown)
            {
                if(qteSOList[keyClickedIndex].keyToPress != buttonPressed)
                {
                    Debug.Log("Wrong button!");
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
            state = State.Idle;
        }
    }

    public override void InteractAlternate(PlayerController player)
    {
        if(HasPickableObject()) 
        { 
            if(GetPickableObject().IsHunted())
            {
                StartCoroutine(exorcisingDelay());
            }
        }
    }

    IEnumerator exorcisingDelay()
    {
        yield return new WaitForSeconds(1f);
        state = State.Exorcising;
    }
}
