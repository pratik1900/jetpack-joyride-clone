using System.Collections;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private void ActivatePowerup(IPowerUp powerUp)
    {
        powerUp.Activate();

        StartCoroutine(
            DeactivateAfterDurationRoutine(
                powerUp.Duration,
                powerUp
            )
        );
    }

    private IEnumerator DeactivateAfterDurationRoutine(float duration, IPowerUp powerUp)
    {
        yield return new WaitForSeconds(duration);
        DeactivatePowerup(powerUp);
    }

    private void DeactivatePowerup(IPowerUp powerUp)
    {
        powerUp.Deactivate();
    }
}