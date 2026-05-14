using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PowerUpManager.Instance.ActivatePowerUp(new ShieldPowerUp());
            Destroy(gameObject);
        }
    }
}