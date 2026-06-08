using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // This script for player movement with InputAction and game events "bool canMove" that enable or disable movement
    // IMPORTANT: This script need PlayerInput component with "Move" action set up in the Input Actions asset

    [Header("Possible features")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private StaminaSystem staminaSystem;
    private Rigidbody rb;
    private CapsuleCollider playerCollider;

    [Header("Settings")]
    [SerializeField] private InputActionReference sprintAction; // Reference sprint action in the Input Actions asset
    [SerializeField] private float moveSpeed = 5f; // Player speed
    [SerializeField] private float sprintMultiplier = 2f; // Sprint speed multiplier
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private bool isGrounded = true; // This is a simple grounded check, you can replace it with a more complex one if needed
    private float maxSpeed;
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private InputActionReference crouchAction;
    [SerializeField] private float playerCollisionHeight = 3.5f;
    [SerializeField] private float crouchCollisionHeightMultiplier = 0.5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    private Vector3 playerColliderCenter;

    [Header("Debug value")]
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isCrouching = false;

    private void OnEnable()
    {
        GameEvents.OnPlayerCanMove += SetCanMove;
        GameEvents.OnPlayerCanSprint += SetCanSprint;
        sprintAction.action.performed += ctx => {isSprinting = true; GameEvents.SetPlayerSprintingState(true);};
        sprintAction.action.canceled += ctx => {isSprinting = false; GameEvents.SetPlayerSprintingState(false);};
        jumpAction.action.performed += ctx => Jump();
        crouchAction.action.performed += ctx => Crouch();
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
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];

        playerColliderCenter = playerCollider.center;

        // Check if StaminaSystem exists - optional dependency
        staminaSystem = GetComponent<StaminaSystem>();

        maxSpeed = moveSpeed * sprintMultiplier; // Calculate max speed for sprinting
    }

    private void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if(!wasGrounded && isGrounded)
        {
            GameEvents.SetPlayerJump(false);
        }

        Move();
    }

    private void Move()
    {
        // If player can't move, return
        if(!canMove) return;

        // Else read input and move player
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        Vector3 directionVector = new Vector3(inputVector.x, 0.0f, inputVector.y);
        directionVector = transform.TransformDirection(directionVector);
        float currentSpeed = inputVector.magnitude * moveSpeed;

        // Apply sprint multiplier if sprinting
        if (isSprinting && canSprint)
        {
            if(staminaSystem == null || staminaSystem.HasStamina) // If no stamina system, allow sprinting. If stamina system exists, check if player has stamina.
            {
                currentSpeed *= sprintMultiplier;
            }
        }
        if(isCrouching)
        {
            currentSpeed *= crouchSpeedMultiplier;
        }

        //Moving player
        rb.linearVelocity = new Vector3(directionVector.x * currentSpeed, rb.linearVelocity.y, directionVector.z * currentSpeed);

        GameEvents.SetPlayerSpeed(currentSpeed / maxSpeed);
    }

    private void Jump()
    {
        if(!isGrounded) return;

        float jumpForce = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        GameEvents.SetPlayerJump(true);
    }

    private void Crouch()
    {
        if(isCrouching)
        {
            // Stand up
            playerCollider.height = playerCollisionHeight;
            playerCollider.center = playerColliderCenter;
            isCrouching = false;
            GameEvents.SetPlayerCrouch(false);
        }
        else
        {
            // Crouch down
            playerCollider.height = playerCollisionHeight * crouchCollisionHeightMultiplier;
            playerCollider.center = new Vector3(playerColliderCenter.x, playerColliderCenter.y * crouchCollisionHeightMultiplier, playerColliderCenter.z);
            isCrouching = true;
            GameEvents.SetPlayerCrouch(true);
        }
    }
}