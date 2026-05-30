using System;

public static class GameEvents
{
    public static event Action<bool> OnPlayerCanMove;
    public static event Action<bool> OnFirstCameraLook;
    public static event Action<bool> OnThirdCameraLook;

    public static void SetPlayerMoveState(bool canMove)
    {
        OnPlayerCanMove?.Invoke(canMove);
    }

    public static void SetFirstCameraLookState(bool canLook)
    {
        OnFirstCameraLook?.Invoke(canLook);
    }

    public static void SetThirdCameraLookState(bool canLook)
    {
        OnThirdCameraLook?.Invoke(canLook);
    }
}
