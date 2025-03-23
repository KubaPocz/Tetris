using System;
using UnityEngine;

public static class Events
{
    public static event Action<float> OnPlayer1MoveInput;
    public static event Action<float> OnPlayer2MoveInput;
    public static event Action<int, int> OnUpdateScore;
    public static event Action<int> OnEndGame;
    public static event Action<int, GameObject> OnUpdateNextBlock;

    public static void SendMoveInput(int playerId, float direction)
    {
        if (playerId == 1)
            OnPlayer1MoveInput?.Invoke(direction);
        else if (playerId == 2)
            OnPlayer2MoveInput?.Invoke(direction);
    }
    public static void UpdateScore(int playerId, int playerScore)
    {
        OnUpdateScore?.Invoke(playerId, playerScore);
    }
    public static void EndGame(int playerId)
    {
        OnEndGame?.Invoke(playerId);
    }
    public static void UpdateNextBlock(int playerId,GameObject nextBlock)
    {
        OnUpdateNextBlock?.Invoke(playerId, nextBlock);
    }
}
