using System;

// [System.Serializable]
// public class PlayerHitTrigger : IExpiryTrigger
// {
//     public event Action OnPowerupExpired;

//     public void Init() => GameEvents.OnPlayerHit += OnPlayerHit;

//     public void Dispose() => GameEvents.OnPlayerHit -= OnPlayerHit;

//     private void OnPlayerHit() => OnPowerupExpired?.Invoke();
// }

public class PlayerHitTrigger : PowerupExpiryTrigger
{
    // public event Action OnPowerupExpired;

    public override void Init() => GameEvents.OnPlayerHit += OnPlayerHit;

    public override void Dispose() => GameEvents.OnPlayerHit -= OnPlayerHit;

    private void OnPlayerHit() => ExpirePowerup();
}