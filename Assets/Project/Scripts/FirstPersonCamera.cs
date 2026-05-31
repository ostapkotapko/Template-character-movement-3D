using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    // This script for player camera first person with InputAction and game events "bool canLook" that enable or disable camera movement
    // IMPORTANT: This script need PlayerInput component with "Look" action set up in the Input Actions asset

    private bool canLook = true;

    private PlayerInput playerInput;
    private InputAction lookAction;
    private float currentVerticalAngle = 0f;

    [SerializeField] private Camera playerCamera; // Reference player camera
    [SerializeField] private float sensitivity = 0.1f; // Sensitivity for camera rotation
    [SerializeField] private float maxVerticalAngle = 80f; // Max vertical angle to prevent flipping

    private void OnEnable()
    {
        GameEvents.OnFirstCameraLook += SetCanLook;
    }

    private void OnDisable()
    {
        GameEvents.OnFirstCameraLook -= SetCanLook;
    }

    private void SetCanLook(bool canLook)
    {
        this.canLook = canLook;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
    }

    private void Look()
    {
        // If player can't look, return
        if(!canLook) return;

        // Else read input and move player camera
        // Rotate player horizontally
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, lookInput.x * sensitivity);

        // Calculate vertical rotation and clamp it
        currentVerticalAngle -= lookInput.y * sensitivity;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -maxVerticalAngle, maxVerticalAngle);
        playerCamera.transform.localEulerAngles = new Vector3(currentVerticalAngle, 0f, 0f);
    }
}