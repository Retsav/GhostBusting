using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHuntedObjectsParent
{
    public static PlayerController Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedTableChangedEventArgs> OnSelectedTableChanged;

    public class OnSelectedTableChangedEventArgs : EventArgs
    {
        public BaseTable selectedTable;
    }


    [SerializeField] private float speed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask tablesLayerMask;

    private PickableObject pickableObject;
    [SerializeField] private Transform pickableObjectHoldPoint;
    [SerializeField] private OccultTable occultTable;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseTable lastSelectedTable;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Cos sie wyjebalo. Nie ma wiecej niz jednego gracza?");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void Update()
    {
        bool allowedMovement = occultTable.GetOccultTableState() != OccultTable.State.Exorcising;
        if(allowedMovement)
        {
            HandleMovement();
            HandleInteractions();
        }
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, tablesLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseTable baseTable))
            {
                if (baseTable != lastSelectedTable)
                {
                    SetSelectedTable(baseTable);
                }
            }
            else
            {
                SetSelectedTable(null);
            }
        }
        else
        {
            SetSelectedTable(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = speed * Time.deltaTime;
        float playerRadius = .6f;
        float playerHeight = 1f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }


    public bool IsWalking()
    {
        return isWalking;
    }
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (lastSelectedTable != null)
        {
            lastSelectedTable.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (lastSelectedTable != null)
        {
            lastSelectedTable.Interact(this);
        }
    }

    private void SetSelectedTable(BaseTable selectedTable)
    {
        lastSelectedTable = selectedTable;
        OnSelectedTableChanged?.Invoke(this, new OnSelectedTableChangedEventArgs
        {
            selectedTable = lastSelectedTable
        });
    }

    public Transform GetPickableObjectFollowTransform()
    {
        return pickableObjectHoldPoint;
    }

    public void SetPickableObject(PickableObject pickableObject)
    {
        this.pickableObject = pickableObject;
        if (pickableObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public PickableObject GetPickableObject()
    {
        return pickableObject;
    }

    public void ClearPickableObject()
    {
        pickableObject = null;
    }

    public bool HasPickableObject()
    {
        return pickableObject != null;
    }
}
