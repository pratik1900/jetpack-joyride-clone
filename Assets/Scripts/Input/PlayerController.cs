using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D playerRb;

    [Header("Movement")]
    [SerializeField] private float thrustSpeed = 5f;
    // [SerializeField] private float fallSpeed = 8f;

    private InputAction thrustAction;

    void Awake()
    {
        playerControls = new PlayerControls();
        playerRb = GetComponent<Rigidbody2D>();

        thrustAction = playerControls.Player.Thrust;
    }

    void Update()
    {
        if (thrustAction.IsPressed())
        {
            playerRb.AddForce(Vector2.up * thrustSpeed, ForceMode2D.Force);
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Destroy(gameObject);
        }
    }
}
