using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    // This script for third person player camera with InputAction and game events "bool canLook" that enable or disable camera movement
    // IMPORTANT: This script need PlayerInput component with "Look" action set up in the Input Actions asset

    [Header("Possible features")]
    [SerializeField] private bool canLook = true;

    private PlayerInput playerInput;
    private InputAction lookAction;
    private float currentVerticalAngle = 0f;

    [Header("Settings")]
    [SerializeField] private GameObject playerCameraPivot; // Reference player camera pivot
    [SerializeField] private float sensitivity = 0.1f; // Sensitivity for camera rotation
    [SerializeField] private float maxVerticalAngle = 80f; // Max vertical angle to prevent flipping

    private void OnEnable()
    {
        GameEvents.OnThirdCameraLook += SetCanLook;
    }

    private void OnDisable()
    {
        GameEvents.OnThirdCameraLook -= SetCanLook;
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
        playerCameraPivot.transform.localEulerAngles = new Vector3(currentVerticalAngle, 0f, 0f);
    }
}
