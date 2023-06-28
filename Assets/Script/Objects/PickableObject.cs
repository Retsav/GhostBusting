using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public static PickableObject Instance { get; private set; }
    [SerializeField] private PickableObjectSO pickableObjectSO;
    [SerializeField] private GameObject pickableObjectToChangeColor;

    public event EventHandler<OnExorcisedObjectEventArgs> OnExorcisedObject;
    public event EventHandler OnObjectDestroy;

    [SerializeField] private float rollDelay = 5f;
    private const int MAX_ROLL = 11;

    public class OnExorcisedObjectEventArgs : EventArgs
    {
        public PickableObjectSO pickableObjectSO;
    }
    

    public enum State
    {
        Idle,
        Hunted,
        Exorcised,
        FailedExorcise,
    }

    private IHuntedObjectsParent pickableObjectParent;
    private State state;


    private void Awake()
    {
        Instance = this;
        state = State.Idle;
    }
    public PickableObjectSO GetPickableObjectSO()
    {
        return pickableObjectSO;
    }

    private void Start()
    {
        StartCoroutine(TryTurningHunted());
    }

    private void Update()
    {
        switch(state)
        {
            case State.Idle:
                break;
            case State.Hunted:
                break;
            case State.Exorcised:
                Debug.Log("Exorcised!");
                OnObjectDestroy?.Invoke(this, EventArgs.Empty);
                DestroySelf();
                break;
            case State.FailedExorcise:
                OnObjectDestroy?.Invoke(this, EventArgs.Empty);
                DestroySelf();
                break;
        }
    }


    IEnumerator TryTurningHunted()
    {
        while(state == State.Idle)
        {
            yield return new WaitForSeconds(rollDelay);
            var roll = Mathf.Floor(UnityEngine.Random.Range(0, MAX_ROLL));
            if (roll >= 10)
            {
                state = State.Hunted;
            }   
        }
    }

    public void ChangePickableObjectState(State state)
    {
        this.state = state;
    }

    public State GetPickableObjectState()
    {
        return state;
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


    public bool IsHunted()
    {
        bool isHunted = state == State.Hunted;
        return isHunted;
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

    public QuickTimeEventsActionListSO GetPickableObjectQTEList()
    {
        return pickableObjectSO.qteList;
    }
}
