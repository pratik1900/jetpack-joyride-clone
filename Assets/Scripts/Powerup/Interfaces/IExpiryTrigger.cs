using System;

public interface IExpiryTrigger
{
    // event Action Triggered;
    event Action OnPowerupExpired;
    void Init();
    void Dispose();
}