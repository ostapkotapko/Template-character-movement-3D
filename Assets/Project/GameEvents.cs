using System;

public static class GameEvents
{
    public static event Action<bool> OnPlayerCanMove;

    public static void SetPlayerMoveState(bool canMove)
    {
        OnPlayerCanMove?.Invoke(canMove);
    }
}
