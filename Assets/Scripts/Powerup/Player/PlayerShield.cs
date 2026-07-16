using System;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public event Action OnShieldBlockedHit;

    public bool IsActive { get; private set; }

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
        // DisableShield();
        GetComponentInChildren<Powerup>().Expire();
        return true;
    }
}

// TO DETERMINE: IS IsActive (and PlayerShield.cs - apart from TryBlockHit) even necessary? WHen shield blocks hit, we simply
// expire the powerup,which destroys the instance anyway
