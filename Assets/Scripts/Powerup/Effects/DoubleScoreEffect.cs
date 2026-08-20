using UnityEngine;

public class DoubleScoreEffect : PowerupEffect
{
    public override void Apply()
    {
        GameEvents.TriggerUpdateScoreMultiplier(2);
    }

    public override void Remove()
    {
        GameEvents.TriggerUpdateScoreMultiplier(1);
    }
}
