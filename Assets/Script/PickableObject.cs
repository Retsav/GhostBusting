using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private PickableObjectSO pickableObjectSO;
    [SerializeField] private GameObject pickableObjectToChangeColor;

    [SerializeField] private float rollDelay = 5f;
    private const int MAX_ROLL = 11;


    public enum State
    {
        Idle,
        Hunted,
    }

    private IHuntedObjectsParent pickableObjectParent;
    private State state;


    private void Awake()
    {
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
                pickableObjectToChangeColor.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
        }
        Debug.Log(state);
    }


    IEnumerator TryTurningHunted()
    {
        while(state == State.Idle)
        {
            yield return new WaitForSeconds(rollDelay);
            var roll = Mathf.Floor(UnityEngine.Random.Range(0, MAX_ROLL));
            Debug.Log(roll);
            if (roll >= 10)
            {
                state = State.Hunted;
            }   
        }
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
}
