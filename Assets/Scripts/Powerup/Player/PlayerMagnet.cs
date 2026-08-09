using System;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    public event Action OnMagnetActivated;

    public bool IsActive { get; private set; }

    public void EnableMagnet()
    {
        IsActive = true;
    }

    public void DisableMagnet()
    {
        IsActive = false;
    }

    public bool TryBlockHit()
    {
        if (!IsActive)
            return false;

        OnMagnetActivated?.Invoke();
        // DisableMagnet();
        GetComponentInChildren<Powerup>().Expire();
        return true;
    }
}

// TO DETERMINE: IS IsActive (and PlayerMagnet.cs - apart from TryBlockHit) even necessary? WHen magnet activates, we simply
// expire the powerup,which destroys the instance anyway
