using System;

public class PlayerHitTrigger : IExpiryTrigger
{
    public event Action OnPowerupExpired;

    public void Init() => GameEvents.OnPlayerHit += OnPlayerHit;

    public void Dispose() => GameEvents.OnPlayerHit -= OnPlayerHit;

    private void OnPlayerHit() => OnPowerupExpired?.Invoke();
}