using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // This script for player movement with InputAction and game events "bool canMove" that enable or disable movement
    // IMPORTANT: This script need PlayerInput component with "Move" action set up in the Input Actions asset

    private bool canMove = true;

    private PlayerInput playerInput;
    private InputAction moveAction;

    [SerializeField] private float moveSpeed = 5f; // Player speed

    private void OnEnable()
    {
        GameEvents.OnPlayerCanMove += SetCanMove;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerCanMove -= SetCanMove;
    }

    private void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
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

        //Moving player
        transform.position += directionVector * directionSpeed;
    }
}