using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // This script for player movement with InputAction and game events "bool canMove" that enable or disable movement
    // IMPORTANT: This script need PlayerInput component with "Move" action set up in the Input Actions asset

    [Header("Possible features")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canSprint = true;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private bool isSprinting = false;

    [Header("Settings")]
    [SerializeField] private InputActionReference sprintAction; // Reference sprint action in the Input Actions asset
    [SerializeField] private float moveSpeed = 5f; // Player speed
    [SerializeField] private float sprintMultiplier = 2f; // Sprint speed multiplier

    private void OnEnable()
    {
        GameEvents.OnPlayerCanMove += SetCanMove;
        GameEvents.OnPlayerCanSprint += SetCanSprint;
        sprintAction.action.performed += ctx => isSprinting = true;
        sprintAction.action.canceled += ctx => isSprinting = false;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerCanMove -= SetCanMove;
        GameEvents.OnPlayerCanSprint -= SetCanSprint;
    }

    private void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void SetCanSprint(bool canSprint)
    {
        this.canSprint = canSprint;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // If player can't move, return
        if(!canMove) return;

        // Else read input and move player
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        Vector3 directionVector = new Vector3(inputVector.x, 0.0f, inputVector.y);
        float directionSpeed = moveSpeed * Time.deltaTime;

        // Apply sprint multiplier if sprinting
        if (isSprinting && canSprint)
        {
            directionSpeed *= sprintMultiplier;
        }

        //Moving player
        transform.position += transform.TransformDirection(directionVector) * directionSpeed;
    }
}