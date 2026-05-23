using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    [SerializeField] private PowerupSO powerupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            powerupEffect.Apply(collision.gameObject);
        }
    }
}