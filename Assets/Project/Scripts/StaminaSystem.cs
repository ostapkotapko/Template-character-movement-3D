using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [SerializeField] private int maxStamina = 1000;
    [SerializeField] private int startStamina = -1; // -1 means start with full stamina

    private int currentStamina;
    private bool isSprinting = false;

    private void OnEnable()
    {
        GameEvents.OnPlayerSprinting += SetSprinting;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerSprinting -= SetSprinting;
    }

    private void Awake()
    {
        if(startStamina == -1)
        {
            currentStamina = maxStamina;
        }
    }

    private void FixedUpdate()
    {
        if(isSprinting && HasStamina)
        {
            currentStamina--;
        }
    }

    private void SetSprinting(bool isSprinting)
    {
        this.isSprinting = isSprinting;
    }

    public bool HasStamina => currentStamina > 0;
}
