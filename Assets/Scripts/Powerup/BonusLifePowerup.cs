using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Bonus Life")]
public class BonusLifePowerup : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        GameEvents.TriggerPlayerLifeGain();
    }
}