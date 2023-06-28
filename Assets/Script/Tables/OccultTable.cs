using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class OccultTable : BaseTable, IHasProgress
{
    public static OccultTable Instance { get; private set; }
    public event EventHandler<OnPickableTableEventArgs> OnSelectedTableGiven;
    public event EventHandler<OnCorrectButtonPressedEventArgs> OnCorrectButtonPressed;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler OnObjectDropped;
    public event EventHandler OnObjectPicked;
    public event EventHandler OnStartedExorcising;
    public event EventHandler OnFinishedExorcising;
    public event EventHandler OnSuccessfulFinishedExorcising;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private int keyClickedIndex;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float t;
    private PickableObjectSO pickableObjectSO;
    public class OnPickableTableEventArgs : EventArgs
    {
        public PickableObjectSO pickableObjectSO;
    }

    public class OnFailedExorciseEventArgs : EventArgs
    {
        public PickableObject pickableObject;
    }

    public class OnCorrectButtonPressedEventArgs : EventArgs
    {
        public QTE_SO qteSO;
    }

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
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
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state,
        });
    }

    private void Update()
    {
        if(state == State.Exorcising)
        {
            t -= Time.deltaTime;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = t / pickableObjectSO.qteTimerMax
            });
            KeyCode buttonPressed = gameInput.GetKeyPressed();
            if(t > 0)
            {
                if (Input.GetKeyDown(qteSOList[keyClickedIndex].keyToPress))
                {
                    OnCorrectButtonPressed?.Invoke(this, new OnCorrectButtonPressedEventArgs
                    {
                        qteSO = qteSOList[keyClickedIndex]
                    });
                    keyClickedIndex++;
                    if (keyClickedIndex == qteSOList.Count)
                    {
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state,
                        });
                        GetPickableObject().ChangePickableObjectState(PickableObject.State.Exorcised);
                        t = 0f;
                        qteSOList.Clear();
                        pickableObjectSO = null;
                        keyClickedIndex = 0;
                        OnSuccessfulFinishedExorcising?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {

                    }
                }
                else if (Input.anyKeyDown)
                {
                    if (qteSOList[keyClickedIndex].keyToPress != buttonPressed)
                    {
                        Debug.Log("Wrong button!");
                        state = State.Idle;
                        t = 0f;
                        pickableObjectSO = null;
                        qteSOList.Clear();
                        keyClickedIndex = 0;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state,
                        });
                        GetPickableObject().ChangePickableObjectState(PickableObject.State.FailedExorcise);
                        OnFinishedExorcising?.Invoke(this, EventArgs.Empty);
                    }
                }
            } else
            {
                t = 0f;
                Debug.Log("Ran out of time!");
                keyClickedIndex = 0;
                state = State.Idle;
                pickableObjectSO = null;
                qteSOList.Clear();
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state,
                });
                GetPickableObject().ChangePickableObjectState(PickableObject.State.FailedExorcise);
                OnFinishedExorcising?.Invoke(this, EventArgs.Empty);
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
                    pickableObjectSO = GetPickableObject().GetPickableObjectSO();
                    t = pickableObjectSO.qteTimerMax;
                    for (int i = 0; i < GetPickableObject().GetPickableObjectQTEList().quickTimeEventActionList.Count; i++)
                    {
                        qteSOList.Add(GetPickableObject().GetPickableObjectQTEList().quickTimeEventActionList[i]);
                    }
                }
            } 
        } else {
            GetPickableObject().SetPickableObjectParent(player);
            pickableObjectSO = null;
            OnObjectPicked?.Invoke(this, EventArgs.Empty);
            t = 0f;
            qteSOList.Clear();
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state,
            });
        }
    }

    public State GetOccultTableState()
    {
        return state;
    }

    public override void InteractAlternate(PlayerController player)
    {
        if(HasPickableObject()) 
        { 
            if(GetPickableObject().IsHunted())
            {
                StartCoroutine(exorcisingDelay());
                OnStartedExorcising?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    IEnumerator exorcisingDelay()
    {
        yield return new WaitForSeconds(1f);
        state = State.Exorcising;
        OnObjectDropped?.Invoke(this, EventArgs.Empty);
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state,
        });
    }
}
