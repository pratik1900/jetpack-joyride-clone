using System;
using UnityEngine;

public class ShieldEffect : PowerupEffect
{
    private PlayerPowerupController powerupController;
    private PlayerController playerController;

    public override void Apply()
    {
        playerController = GetComponentInParent<PlayerController>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard"))
        {
            Debug.Log("Check 2");
            playerController.ProcessHazardHit(collision);
        }
    }
}
