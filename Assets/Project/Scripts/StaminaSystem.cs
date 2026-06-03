using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxStamina = 1000;
    [SerializeField] private float startStamina = -1; // -1 means start with full stamina
    [SerializeField] private float restoreTime = 1; // Time in seconds to start restore stamina
    [SerializeField] private float restoreInterval = 0.01f; // Time in seconds between each stamina restore

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
            timer = 0;
            timerInterval = 0;
        }
        else if(!isSprinting && currentStamina < maxStamina && timer < restoreTime)
        {
            timer += Time.fixedDeltaTime;
        }
        else if(!isSprinting && currentStamina < maxStamina && timer >= restoreTime)
        {
            timerInterval += Time.fixedDeltaTime;
            if(timerInterval >= restoreInterval)
            {
                currentStamina += timerInterval / restoreInterval;
                timerInterval = 0;
            }
        }
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    private void SetSprinting(bool isSprinting)
    {
        this.isSprinting = isSprinting;
        if(isSprinting) timer = 0;
    }

    public bool HasStamina => currentStamina > 0;
}
