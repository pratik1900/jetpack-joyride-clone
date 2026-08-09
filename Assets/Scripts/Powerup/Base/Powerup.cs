using System.Collections;
using UnityEngine;

#nullable enable

public class Powerup : MonoBehaviour
{
    // References
    [SerializeField]
    private float timer;

    [SerializeField]
    private PowerupExpiryTrigger? expiryTrigger;

    [SerializeField]
    private PowerupEffect effect;

    [SerializeField]
    public Sprite icon;

    // Properties (for access)
    // public IEffect Effect => effect;
    private bool _isExpired = false;
    private IEnumerator? _expiryCoroutine;

    // Call this when the player picks up the powerup
    public void Activate()
    {
        // APPLY EFFECT
        effect?.Apply();

        // SET UP EXPIRY

        // For Instant Buffs
        if (timer <= 0 && expiryTrigger == null)
        {
            Expire();
            return;
        }
        // For Timer-Based Buffs
        if (timer > 0)
        {
            _expiryCoroutine = ExpiryTimer(timer);
            StartCoroutine(_expiryCoroutine);
        }

        // For Condition-Based Buffs (listenners)
        if (expiryTrigger != null)
        {
            expiryTrigger.Init();
            expiryTrigger.OnPowerupExpired += Expire; // setup listener for destroying the powerup once expiry condition is met
        }
    }

    private IEnumerator ExpiryTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Expire();
    }

    public void RefreshExpiryTimerIfPresent()
    {
        if (_expiryCoroutine == null || timer <= 0)
        {
            return;
        }

        StopCoroutine(_expiryCoroutine);
        _expiryCoroutine = ExpiryTimer(timer);
        StartCoroutine(_expiryCoroutine);
    }

    public void Expire()
    {
        if (_isExpired)
            return;

        _isExpired = true;
        // Undo the effects of the powerup
        effect?.Remove();

        if (expiryTrigger != null)
        {
            expiryTrigger.OnPowerupExpired -= Expire;
            expiryTrigger.Dispose();
        }

        StopAllCoroutines();
        // Destroy(this);
        Destroy(gameObject); //for the prefab-approach
    }
}
