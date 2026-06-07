using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private bool haveAnimations = false;

    private float playerSpeed = 0f;
    private bool playerJumping = false;
    private Animator animator;

    private void OnEnable()
    {
        GameEvents.OnPlayerSpeedChanged += SetPlayerSpeed;
        GameEvents.OnPlayerJumping += SetPlayerJump;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerSpeedChanged -= SetPlayerSpeed;
        GameEvents.OnPlayerJumping -= SetPlayerJump;
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

    private void SetPlayerJump(bool isJumping)
    {
        playerJumping = isJumping;
        animator.SetBool("IsJumping", playerJumping);
    }
}
