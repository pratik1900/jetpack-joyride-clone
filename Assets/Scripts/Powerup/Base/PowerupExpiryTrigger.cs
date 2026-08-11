using System;
using UnityEngine;

public abstract class PowerupExpiryTrigger : MonoBehaviour
{
    public event Action OnPowerupExpired;
    public abstract void Init();
    public abstract void Dispose();

    protected void ExpirePowerup()
    {
        OnPowerupExpired?.Invoke();
    }
}
