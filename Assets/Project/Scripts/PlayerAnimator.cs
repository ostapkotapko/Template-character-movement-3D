using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private bool haveAnimations = false;

    private float playerSpeed = 0f;
    private Animator animator;

    private void OnEnable()
    {
        GameEvents.OnPlayerSpeedChanged += SetPlayerSpeed;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerSpeedChanged -= SetPlayerSpeed;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(animator == null && haveAnimations)
        {
            Debug.LogError("PlayerAnimator: No Animator component found on the GameObject.");
        }
    }

    private void Update()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", playerSpeed);
    }

    private void SetPlayerSpeed(float speed)
    {
        playerSpeed = speed;
    }
}
