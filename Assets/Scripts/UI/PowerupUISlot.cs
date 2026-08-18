using UnityEngine;

// Not sure about this
public class PowerupUISlot : MonoBehaviour
{
    public PowerupType powerupType;
    public bool hasTimer;
    public float remainingTimePercent;

    private GameObject timerBar;
    private UnityEngine.UI.Image timerBarImage;

    private void Start()
    {
        timerBar = transform.GetChild(0).gameObject;

        if (timerBar != null && hasTimer)
        {
            timerBar.SetActive(true);
            timerBarImage = timerBar.GetComponent<UnityEngine.UI.Image>();
        }
        else
        {
            timerBar.SetActive(false);
        }
    }

    private void Update()
    {
        if (hasTimer && timerBarImage != null)
        {
            timerBarImage.fillAmount = remainingTimePercent;
        }
    }
}
