using System;
using UnityEngine;

public class MagnetEffect : PowerupEffect
{
    private PlayerPowerupController powerupController;
    private PlayerController playerController;

    public override void Apply()
    {
        playerController = GetComponentInParent<PlayerController>();
        powerupController = GetComponentInParent<PlayerPowerupController>();

        if (powerupController == null)
        {
            Debug.LogWarning("MagnetEffect could not find PlayerPowerupController.");
            return;
        }

        if (powerupController.Magnet == null)
        {
            Debug.LogWarning("MagnetEffect could not find PlayerMagnet.");
            return;
        }

        powerupController.Magnet.EnableMagnet();
    }

    public override void Remove()
    {
        if (powerupController == null)
        {
            return;
        }

        powerupController.Magnet.DisableMagnet();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.CompareTag("Hazard"))
        // {
        //     Debug.Log("Check 2");
        //     playerController.ProcessHazardHit(collision);
        // }

        if (collision.CompareTag("Coin"))
        {
            collision.GetComponent<Coin>().StartMovingTowardsPlayer();
        }
    }
}
