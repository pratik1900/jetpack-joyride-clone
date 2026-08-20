using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupController : MonoBehaviour
{
    // [SerializeField]
    private Transform activePowerupRoot;

    // [SerializeField]
    private PlayerShield shield;

    // [SerializeField]
    private PlayerMagnet magnet;

    // Properties (for access)
    public Transform ActivePowerupRoot => activePowerupRoot != null ? activePowerupRoot : transform;
    public PlayerShield Shield => shield;
    public PlayerMagnet Magnet => magnet;

    private readonly Dictionary<PowerupType, Powerup> activePowerups = new();

    //For UI
    public IReadOnlyDictionary<PowerupType, Powerup> ActivePowerups => activePowerups;

    private void Awake()
    {
        if (activePowerupRoot == null)
        {
            activePowerupRoot = transform.Find("ActivePowerups");
        }

        if (shield == null)
        {
            shield = GetComponent<PlayerShield>();
        }

        if (magnet == null)
        {
            magnet = GetComponent<PlayerMagnet>();
        }
    }

    public void RegisterActivePowerup(Powerup powerup)
    {
        if (powerup == null)
        {
            return;
        }

        activePowerups[powerup.Type] = powerup;

        //  For updating the powerup UI slots
        GameEvents.TriggerPowerupActivated();
    }

    public void UnregisterActivePowerup(Powerup powerup)
    {
        if (powerup == null)
        {
            return;
        }

        if (
            activePowerups.TryGetValue(powerup.Type, out Powerup activePowerup)
            && activePowerup == powerup
        )
        {
            activePowerups.Remove(powerup.Type);

            //  For updating the powerup UI slots
            GameEvents.TriggerPowerupExpired(powerup.Type);
        }
    }

    public bool TryGetActivePowerup(PowerupType type, out Powerup powerup)
    {
        return activePowerups.TryGetValue(type, out powerup);
    }
}
