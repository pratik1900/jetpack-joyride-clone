using UnityEngine;

[CreateAssetMenu(menuName = "Powerup/Powerup Definition Data Asset")]
public sealed class PowerupDefinition : ScriptableObject
{
    // References
    [SerializeField] private float timer;
    [SerializeReference] private IExpiryTrigger? expiryTrigger;
    [SerializeReference] private IEffect effect;

    [SerializeField] private Sprite icon;

    // Properties (for access)
    public IEffect Effect => effect;

    public void Init(ActivePowerup powerup)
    {
        effect.Apply();

        // For Instant Buffs
        if (timer <= 0 && expiryTrigger == null)
        {
            powerup.Expire();
            return;
        }

        // Setup Expiry Timer
        if (timer > 0)
            powerup.StartExpiryTimer(timer);

        // Setup expiry condition (listenners)
        if (expiryTrigger != null)
        {
            expiryTrigger.Init();
            expiryTrigger.OnPowerupExpired += powerup.Expire; // setup listener for destroying the powerup once expiry condition is met 
            powerup.SetTrigger(expiryTrigger);
        }
    }
}
