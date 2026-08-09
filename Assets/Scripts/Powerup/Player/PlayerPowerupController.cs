using UnityEngine;

public class PlayerPowerupController : MonoBehaviour
{
    // [SerializeField]
    private Transform activePowerupRoot;

    // [SerializeField]
    private PlayerShield shield;

    // [SerializeField]
    private PlayerMagnet magnet;

    // Properties (for access)
    public Transform ActivePowerupRoot => activePowerupRoot != null ? activePowerupRoot : transform;
    public PlayerShield Shield => shield;
    public PlayerMagnet Magnet => magnet;

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

        if (magnet == null)
        {
            magnet = GetComponent<PlayerMagnet>();
        }
    }
}
