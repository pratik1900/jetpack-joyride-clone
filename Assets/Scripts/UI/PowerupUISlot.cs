using UnityEngine;

public class PowerupUISlot : MonoBehaviour
{
    public PowerupType powerupType;
    public bool hasTimer;
    public float remainingTimePercent;

    private GameObject timerBar;
    private UnityEngine.UI.Image timerBarImage;

    private PlayerPowerupController playerPowerupController;
    private Powerup powerupData;

    private void Start()
    {
        playerPowerupController = Object.FindAnyObjectByType<PlayerPowerupController>();
        timerBar = transform.GetChild(0).GetChild(1).gameObject;

        if (timerBar != null && hasTimer)
        {
            timerBar.SetActive(true);
            timerBarImage = timerBar.GetComponent<UnityEngine.UI.Image>();
        }
        else
        {
            timerBar.SetActive(false);
        }

        BindPowerupData();
    }

    private void Update()
    {
        if (hasTimer)
        {
            if (timerBarImage == null)
            {
                Debug.LogWarning("Timer bar image is not assigned.");
                return;
            }

            remainingTimePercent = powerupData.RemainingTimePercent;
            timerBarImage.fillAmount = remainingTimePercent;
        }
    }

    private void BindPowerupData()
    {
        if (
            playerPowerupController != null
            && playerPowerupController.ActivePowerups.TryGetValue(
                powerupType,
                out Powerup powerupData
            )
        )
        {
            this.powerupData = powerupData;
        }
    }
}
