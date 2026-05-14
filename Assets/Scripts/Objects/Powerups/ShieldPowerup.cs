using UnityEngine;

public class ShieldPowerUp : IPowerUp
{
    public PowerUpType Type => PowerUpType.Shield;
    public float Duration => 5f;

    public void Activate() => GameEvents.TriggerShieldActivated();
    public void Deactivate() => GameEvents.TriggerShieldDeactivated();
    public void ForceDeactivate() => GameEvents.TriggerShieldDeactivated();
}