using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Bonus Life")]
public class BonusLifePowerup : PowerupSO
{
    public override void Apply(GameObject target)
    {
        LifeManager.Instance.AddLife();
    }
}