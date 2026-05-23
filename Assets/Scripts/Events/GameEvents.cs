using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnPlayerHit;
    public static event Action OnGameOver;
    public static event Action<int> OnLivesChanged;

    // Trigger functions (used because the actions can only be invoked inside the class they are defined in, when we use the 'event' keyword while defining them. The 'event' keyword is necessary to protect the actions from accidental overwrites from other classes)
    public static void TriggerPlayerHit() => OnPlayerHit?.Invoke();
    public static void TriggerGameOver() => OnGameOver?.Invoke();
    public static void TriggerLivesChanged(int lives) => OnLivesChanged?.Invoke(lives);
}
