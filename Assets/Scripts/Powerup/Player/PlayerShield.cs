using System;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public event Action OnShieldBlockedHit;

    public bool IsActive { get; private set; }
    private PlayerPowerupController powerupController;

    private void Awake()
    {
        powerupController = GetComponent<PlayerPowerupController>();
    }

    public void EnableShield()
    {
        IsActive = true;
    }

    public void DisableShield()
    {
        IsActive = false;
    }

    public bool TryBlockHit()
    {
        if (!IsActive)
            return false;

        OnShieldBlockedHit?.Invoke();
        if (powerupController != null && powerupController.TryGetActivePowerup(PowerupType.Shield, out Powerup shieldPowerup))
        {
            shieldPowerup.Expire();
        }
        else
        {
            DisableShield();
        }

        return true;
    }
}

// TO DETERMINE: IS IsActive (and PlayerShield.cs - apart from TryBlockHit) even necessary? When shield blocks hit, we simply
// expire the powerup, which destroys the instance anyway
