using System;
using UnityEngine;

// public class ShieldEffect : PowerupEffect
// {
//     public event Action OnShieldEnabled;
//     public event Action OnShieldDisabled;

//     public override void Apply()
//     {
//         OnShieldEnabled?.Invoke();
//     }
//     public override void Remove()
//     {
//         OnShieldDisabled?.Invoke();
//     }
// }

public class ShieldEffect : PowerupEffect
{
    private PlayerPowerupController powerupController;

    public override void Apply()
    {
        powerupController = GetComponentInParent<PlayerPowerupController>();

        if (powerupController == null)
        {
            Debug.LogWarning("ShieldEffect could not find PlayerPowerupController.");
            return;
        }

        powerupController.Shield.EnableShield();
    }

    public override void Remove()
    {
        if (powerupController == null)
        {
            return;
        }

        powerupController.Shield.DisableShield();
    }
}
