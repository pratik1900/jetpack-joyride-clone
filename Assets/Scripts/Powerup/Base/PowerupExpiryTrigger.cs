using System;
using UnityEngine;

// public interface IExpiryTrigger
// {
//     // event Action Triggered;
//     event Action OnPowerupExpired;
//     void Init();
//     void Dispose();
// }

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
