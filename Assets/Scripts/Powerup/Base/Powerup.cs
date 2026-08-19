using System.Collections;
using UnityEngine;

#nullable enable

public enum PowerupType
{
    Shield,
    Magnet,
    Life,
}

public class Powerup : MonoBehaviour
{
    // References
    [SerializeField]
    private PowerupType type;

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
    public PowerupType Type => type;
    public Sprite Icon => icon;

    private bool _isExpired = false;
    private IEnumerator? _expiryCoroutine;
    private PlayerPowerupController? _powerupController;

    //For UI
    private float expiryTime;

    // public float RemainingTime => Mathf.Max(0f, expiryTime - Time.time);
    // public float RemainingTimePercent => timer > 0f ? RemainingTime / timer : 0f;
    public float RemainingTimePercent =>
        timer > 0f ? Mathf.Max(0f, expiryTime - Time.time) / timer : 0f;

    public bool HasTimer => timer > 0f;

    // Call this when the player picks up the powerup
    public void Activate()
    {
        _powerupController = GetComponentInParent<PlayerPowerupController>();

        // APPLY EFFECT
        effect?.Apply();

        _powerupController?.RegisterActivePowerup(this);

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

            //for UI
            expiryTime = Time.time + timer;
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

        //for UI
        expiryTime = Time.time + timer;
    }

    public void Expire()
    {
        if (_isExpired)
            return;

        _isExpired = true;
        _powerupController?.UnregisterActivePowerup(this);

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
