using System;

[System.Serializable]
public class ShieldEffect : IEffect
{
    public event Action OnShieldEnabled;
    public event Action OnShieldDisabled;


    public void Apply()
    {
        OnShieldEnabled?.Invoke();
    }
    public void Remove()
    {
        OnShieldDisabled?.Invoke();
    }
}