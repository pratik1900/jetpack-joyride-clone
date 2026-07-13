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
        DisableShield();
        return true;
    }
}
