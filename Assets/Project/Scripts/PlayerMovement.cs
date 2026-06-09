using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player movement: walking, sprinting, jumping, crouching.
/// Requires: Rigidbody, CapsuleCollider, PlayerInput with "InputPlayerMovement" actions
/// Listens to: GameEvents.OnPlayerCanMove, GameEvents.OnPlayerCanSprint
/// Broadcasts: GameEvents.SetPlayerSprintingState, SetPlayerSpeed, SetPlayerJump, SetPlayerCrouch
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [Header("Feature Toggles")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private InputActionReference sprintAction;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Crouch")]
    [SerializeField] private float playerCollisionHeight = 3.5f;
    [SerializeField] private float crouchCollisionHeightMultiplier = 0.5f;
    [SerializeField] private InputActionReference crouchAction;

    [Header("Debug value")]
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isCrouching = false;
    [SerializeField] private bool isGrounded = true;

    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    private InputAction moveAction;
    private StaminaSystem staminaSystem; // optional - sprint works without it
    private Vector3 playerColliderCenter;
    private float maxSpeed;    
    

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        moveAction = GetComponent<PlayerInput>().actions["Move"];
        staminaSystem = GetComponent<StaminaSystem>();

        playerColliderCenter = playerCollider.center;
        maxSpeed = moveSpeed * sprintMultiplier;
    }

    private void Update()
    {
        CheckGrounded();
        Move();
    }

    private void CheckGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Notify landing only on the frame we touch the ground
        if(!wasGrounded && isGrounded)
        {
            GameEvents.SetPlayerJump(false);
        }
    }

    private void Move()
    {
        if(!canMove) return;

        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        Vector3 directionVector = new Vector3(inputVector.x, 0.0f, inputVector.y);
        directionVector = transform.TransformDirection(directionVector);
        float currentSpeed = inputVector.magnitude * moveSpeed;

        if (isSprinting && canSprint)
        {
            // If no StaminaSystem attached, sprinting is always allowed
            if(staminaSystem == null || staminaSystem.HasStamina())
            {
                currentSpeed *= sprintMultiplier;
            }
        }
        if(isCrouching)
        {
            currentSpeed *= crouchSpeedMultiplier;
        }

        rb.linearVelocity = new Vector3(directionVector.x * currentSpeed, rb.linearVelocity.y, directionVector.z * currentSpeed);

        // Normalized speed for animator
        GameEvents.SetPlayerSpeed(currentSpeed / maxSpeed);
    }

    private void Jump()
    {
        if(!isGrounded) return;

        // Calculate force needed to reach jumpHeight using kinematics: v = sqrt(2gh)
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

    private void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void SetCanSprint(bool canSprint)
    {
        this.canSprint = canSprint;
    }
}