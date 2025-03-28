using System;
using UnityEngine;

public static class Events
{
    // Player movement input events
    public static event Action<float> OnPlayer1MoveInput;
    public static event Action<float> OnPlayer2MoveInput;

    // Score update event
    public static event Action<int, int> OnUpdateScore;

    // End game event
    public static event Action<int> OnEndGame;

    // Next block preview update event
    public static event Action<int, GameObject> OnUpdateNextBlock;

    // Send movement input based on player ID
    public static void SendMoveInput(int playerId, float direction)
    {
        if (playerId == 1)
            OnPlayer1MoveInput?.Invoke(direction);
        else if (playerId == 2)
            OnPlayer2MoveInput?.Invoke(direction);
    }

    // Trigger score update event
    public static void UpdateScore(int playerId, int playerScore)
    {
        OnUpdateScore?.Invoke(playerId, playerScore);
    }

    // Trigger end game event
    public static void EndGame(int playerId)
    {
        OnEndGame?.Invoke(playerId);
    }

    // Trigger next block update event
    public static void UpdateNextBlock(int playerId, GameObject nextBlock)
    {
        OnUpdateNextBlock?.Invoke(playerId, nextBlock);
    }
}
