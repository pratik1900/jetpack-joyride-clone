using UnityEngine;

// public interface IEffect
// {
//     public void Apply();
//     public void Remove();
// }

public abstract class PowerupEffect : MonoBehaviour
{
    public abstract void Apply();
    public abstract void Remove();
}