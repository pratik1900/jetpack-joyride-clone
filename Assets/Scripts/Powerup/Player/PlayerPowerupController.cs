using UnityEngine;

public class PlayerPowerupController : MonoBehaviour
{
    // [SerializeField]
    private Transform activePowerupRoot;

    // [SerializeField]
    private PlayerShield shield;

    // Properties (for access)
    public Transform ActivePowerupRoot => activePowerupRoot != null ? activePowerupRoot : transform;
    public PlayerShield Shield => shield;

    private void Awake()
    {
        if (activePowerupRoot == null)
        {
            activePowerupRoot = transform.Find("ActivePowerups");
        }

        if (shield == null)
        {
            shield = GetComponent<PlayerShield>();
        }
    }
}
