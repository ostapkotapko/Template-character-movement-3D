using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private InputActionReference switchCameraAction; // Reference switch camera action in the Input Actions asset

    private void OnEnable()
    {
        switchCameraAction.action.performed += OnSwitchCamera;
    }

    private void OnDisable()
    {
        switchCameraAction.action.performed -= OnSwitchCamera;
    }

    private void OnSwitchCamera(InputAction.CallbackContext context)
    {
        SwitchPersonCamera();
    }

    private void Start()
    {
        // Ensure only one camera is active at the start
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
    }

    private void SwitchPersonCamera()
    {
        // Toggle the enabled state of both cameras
        if(firstPersonCamera.enabled)
        {
            // Switch to third-person camera
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
        }
        else
        {
            // Switch to first-person camera
            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
        }
    }
}