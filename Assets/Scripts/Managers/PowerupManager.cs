using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    //Tracks references to coroutines of active powerups (of each type) 
    private Dictionary<PowerUpType, Coroutine> _activeCoroutines = new();
    //Tracks references to instances of active powerups (of each type) 
    private Dictionary<PowerUpType, IPowerUp> _activePowerUps = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ActivatePowerUp(IPowerUp powerUp)
    {
        PowerUpType type = powerUp.Type;

        //Stop existing powerup of same type
        ForceDeactivatePowerUp(type);

        powerUp.Activate();

        // Store References
        _activePowerUps[type] = powerUp;
        _activeCoroutines[type] = StartCoroutine(
            DeactivateAfterExpiryRoutine(
                powerUp
            )
        );
    }

    // Normal Deactivation after expiry
    private IEnumerator DeactivateAfterExpiryRoutine(IPowerUp powerUp)
    {
        yield return new WaitForSeconds(powerUp.Duration);
        powerUp.Deactivate();
        _activeCoroutines.Remove(powerUp.Type);
        _activePowerUps.Remove(powerUp.Type);
    }

    // Forced Deactivation of Coroutines before they expire (e.g. removal of shield after getting hit)
    public void ForceDeactivatePowerUp(PowerUpType type)
    {
        if (_activePowerUps.TryGetValue(type, out IPowerUp powerUp))
        {
            if (_activeCoroutines.TryGetValue(type, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                _activeCoroutines.Remove(type);
            }
            // powerUp.ForceDeactivate();
            powerUp.Deactivate();
            _activePowerUps.Remove(type);
        }
    }
}