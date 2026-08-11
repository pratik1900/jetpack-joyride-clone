using UnityEngine;

public class LifeEffect : PowerupEffect
{
    public override void Apply()
    {
        GameEvents.TriggerPlayerLifeGain();
    }

    public override void Remove()
    {
        // No removal logic for life effect
    }
}
