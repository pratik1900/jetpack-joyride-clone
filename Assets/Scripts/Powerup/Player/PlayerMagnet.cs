using System;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    public event Action OnMagnetActivated;

    public bool IsActive { get; private set; }
    private PlayerPowerupController powerupController;

    private void Awake()
    {
        powerupController = GetComponent<PlayerPowerupController>();
    }

    public void EnableMagnet()
    {
        IsActive = true;
    }

    public void DisableMagnet()
    {
        IsActive = false;
    }
}

// TO DETERMINE: IS IsActive (and PlayerMagnet.cs - apart from TryBlockHit) even necessary? When magnet activates, we simply
// expire the powerup, which destroys the instance anyway
