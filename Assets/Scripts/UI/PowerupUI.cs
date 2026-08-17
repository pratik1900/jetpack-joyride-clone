using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUI : MonoBehaviour
{
    [SerializeField]
    PowerupUISlot[] activePowerups;

    private PlayerPowerupController playerPowerupController;

    private void Start()
    {
        GameEvents.OnPowerupActivated += UpdateActivePowerups;
        GameEvents.OnPowerupExpired += UpdateActivePowerups;

        playerPowerupController = Object.FindAnyObjectByType<PlayerPowerupController>();
    }

    private void Update()
    {
        foreach (var slot in activePowerups)
        {
            if (slot.hasTimer && slot.timerBarImage != null)
            {
                if (
                    playerPowerupController.ActivePowerups.TryGetValue(
                        slot.powerupType,
                        out Powerup powerupData
                    )
                )
                {
                    slot.timerBarImage.fillAmount = powerupData.RemainingTimePercent;
                }
            }
        }
    }

    private void UpdateActivePowerups()
    {
        if (playerPowerupController == null)
        {
            Debug.LogWarning("PlayerPowerupController not found in the scene.");
            return;
        }

        var activePowerupsDict = playerPowerupController.ActivePowerups;

        for (int i = 0; i < activePowerupsDict.Count; i++)
        {
            var powerupType = activePowerupsDict.Keys.ElementAt(i);
            var powerupData = activePowerupsDict[powerupType];

            // Find the corresponding slot for this powerup type
            var slot = System.Array.Find(activePowerups, s => s.powerupType == powerupType);
            if (slot != null)
            {
                // reset timer ui
                // slot.durationSlider.value = powerupData.RemainingDuration;
            }
            else
            {
                PowerupUISlot newSlot = new PowerupUISlot
                {
                    powerupType = powerupType,
                    iconSprite = powerupData.Icon, // Assign the appropriate Image component
                    timerBarImage = null, // Assign the appropriate Image component
                    hasTimer = powerupData.HasTimer,
                };
            }
        }
    }
}

[System.Serializable]
public class PowerupUISlot
{
    public PowerupType powerupType;
    public Sprite iconSprite;
    public Image timerBarImage;
    public bool hasTimer;
}
