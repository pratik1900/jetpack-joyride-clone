using UnityEngine;

public class ShieldPowerup : MonoBehaviour, IPowerUp
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Activate();
        }
    }

    private float duration = 5.0f;

    public float Duration
    {
        get { return duration; }
    }

    public void Activate()
    {
        GameEvents.TriggerShieldToggled(true);
    }

    public void Deactivate()
    {
        GameEvents.TriggerShieldToggled(false);
    }
}