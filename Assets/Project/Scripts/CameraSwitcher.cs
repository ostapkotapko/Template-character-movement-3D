using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
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
        if (firstPersonCamera.enabled)
        {
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
        }
        else
        {
            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
        }
    }
}