using UnityEngine;

/// <summary>
/// Controls stamina value: drains, restores.
/// Listens to: GameEvents.OnPlayerSprinting
/// </summary>

public class StaminaSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxStamina = 1000;
    [SerializeField] private float startStamina = -1; // -1 means start with full stamina
    [SerializeField] private float restoreTime = 1; // Seconds to start restore stamina
    [SerializeField] private float restoreInterval = 0.01f; // Seconds between each stamina restore

    [Header("Debug value")]
    [SerializeField] private float currentStamina;
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private float timer = 0;
    [SerializeField] private float timerInterval = 0;

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
        // Initialize stamina
        if(startStamina == -1)
        {
            currentStamina = maxStamina;
        }
        else
        {
            currentStamina = startStamina;
        }
    }

    private void FixedUpdate()
    {
        // Handle stamina drain and restore
        if(isSprinting && HasStamina())
        {
            currentStamina--;
            timer = 0;
            timerInterval = 0;
        }
        else if(!isSprinting && currentStamina < maxStamina)
        {
            // Wait before restoring
            if(timer < restoreTime)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                // Restores stamina points proportionally to accumulated time
                timerInterval += Time.fixedDeltaTime;
                if(timerInterval >= restoreInterval)
                {
                    currentStamina += timerInterval / restoreInterval;
                    timerInterval = 0;
                }
            }
        }
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    private void SetSprinting(bool isSprinting)
    {
        this.isSprinting = isSprinting;
        if(isSprinting) timer = 0;
    }

    public bool HasStamina()
    {
        return currentStamina > 0;
    }
}
