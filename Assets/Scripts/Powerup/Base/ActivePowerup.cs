using System;
using System.Collections;
using UnityEngine;

public class ActivePowerup : MonoBehaviour
{
    private PowerupDefinition _definition;
    private IExpiryTrigger _expiryTrigger;

    // Call this when the player picks up the powerup
    public void Activate(PowerupDefinition definition)
    {
        _definition = definition;
        _definition.Init(this);
    }

    // these are public so the definition can set up this object
    public void StartExpiryTimer(float duration)
    {
        StartCoroutine(ExpiryTimer(duration));
    }

    public void SetTrigger(IExpiryTrigger expiryTrigger)
    {
        _expiryTrigger = expiryTrigger;
    }

    private IEnumerator ExpiryTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Expire();
    }

    // public so the definition can call it for in case of instant buff type
    public void Expire()
    {
        // Undo the effects of the powerup
        _definition.Effect.Remove();

        if (_expiryTrigger != null)
        {
            _expiryTrigger.OnPowerupExpired -= Expire;
            _expiryTrigger.Dispose();
        }

        StopAllCoroutines();
        Destroy(this);
    }
}