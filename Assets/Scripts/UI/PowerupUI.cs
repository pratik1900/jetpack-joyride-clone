using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUI : MonoBehaviour
{
    [SerializeField]
    PowerupUISlot[] activePowerupUISlots; // Array to hold references to the active powerup UI slots (game objects)

    // PowerupUISlot[] activePowerups;

    private PlayerPowerupController playerPowerupController;

    [SerializeField]
    private GameObject powerupUISlotPrefab;

    private GameObject powerupUISlotsContainer;

    private void Start()
    {
        GameEvents.OnPowerupActivated += UpdateActivePowerups;
        GameEvents.OnPowerupExpired += UpdateActivePowerups;

        playerPowerupController = Object.FindAnyObjectByType<PlayerPowerupController>();
        powerupUISlotsContainer = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        foreach (var slot in activePowerupUISlots)
        {
            if (slot.hasTimer)
            {
                if (
                    playerPowerupController.ActivePowerups.TryGetValue(
                        slot.powerupType,
                        out Powerup powerupData
                    )
                )
                {
                    slot.remainingTimePercent = powerupData.RemainingTimePercent;
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

        activePowerupUISlots = powerupUISlotsContainer.GetComponentsInChildren<PowerupUISlot>();

        foreach (var activePowerup in activePowerupsDict)
        {
            var powerupType = activePowerup.Key;
            var powerupData = activePowerup.Value;

            // Now i need a way to connect the activePowerup from the dictionary to the corresponding UI slot in activePowerupUISlots.
            var uiSlot = activePowerupUISlots.FirstOrDefault(s => s.powerupType == powerupType);
            if (uiSlot != null)
            {
                RefreshPowerupUISlot(uiSlot, powerupData);
            }
            else
            {
                // If no existing slot found, create a new UI slot for the active powerup
                CreatePowerupUISlot(powerupType, powerupData);
            }
        }
    }

    private void CreatePowerupUISlot(PowerupType powerupType, Powerup powerupData)
    {
        GameObject newSlot = Instantiate(powerupUISlotPrefab, powerupUISlotsContainer.transform);
        PowerupUISlot newPowerupUISlot = newSlot.GetComponent<PowerupUISlot>();
        newPowerupUISlot.powerupType = powerupType;
        newPowerupUISlot.hasTimer = powerupData.HasTimer;
        if (powerupData.HasTimer)
        {
            newPowerupUISlot.remainingTimePercent = powerupData.RemainingTimePercent;
        }
        Image iconImage = newSlot.transform.GetComponent<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = powerupData.Icon;
        }
    }

    private void RefreshPowerupUISlot(PowerupUISlot uiSlot, Powerup powerupData)
    {
        if (powerupData.HasTimer)
        {
            uiSlot.remainingTimePercent = powerupData.RemainingTimePercent;
        }
    }
}
