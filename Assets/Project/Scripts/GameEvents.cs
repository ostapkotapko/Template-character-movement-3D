using System;

public static class GameEvents
{
    public static event Action<bool> OnPlayerCanMove;
    public static event Action<bool> OnPlayerCanSprint;
    public static event Action<bool> OnPlayerSprinting;
    public static event Action<bool> OnFirstCameraLook;
    public static event Action<bool> OnThirdCameraLook;
    public static event Action<float> OnPlayerSpeedChanged;
    public static event Action<bool> OnPlayerJumping;
    public static event Action<bool> OnPlayerCrouching;

    public static void SetPlayerMoveState(bool canMove)
    {
        OnPlayerCanMove?.Invoke(canMove);
    }

    public static void SetPlayerSprintState(bool canSprint)
    {
        OnPlayerCanSprint?.Invoke(canSprint);
    }

    public static void SetFirstCameraLookState(bool canLook)
    {
        OnFirstCameraLook?.Invoke(canLook);
    }

    public static void SetThirdCameraLookState(bool canLook)
    {
        OnThirdCameraLook?.Invoke(canLook);
    }

    public static void SetPlayerSprintingState(bool isSprinting)
    {
        OnPlayerSprinting?.Invoke(isSprinting);
    }

    public static void SetPlayerSpeed(float normalizedSpeed)
    {
        OnPlayerSpeedChanged?.Invoke(normalizedSpeed);
    }

    public static void SetPlayerJump(bool isGrounded)
    {
        OnPlayerJumping?.Invoke(isGrounded);
    }

    public static void SetPlayerCrouch(bool isCrouching)
    {
        OnPlayerCrouching?.Invoke(isCrouching);
    }
}
