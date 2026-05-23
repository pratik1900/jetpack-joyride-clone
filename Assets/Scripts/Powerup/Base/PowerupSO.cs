using UnityEngine;

public abstract class PowerupSO : ScriptableObject
{
    public abstract void Apply(GameObject target);
}