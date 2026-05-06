using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D playerRb;

    [Header("Movement")]
    [SerializeField] private float thrustSpeed = 5f;
    // [SerializeField] private float fallSpeed = 8f;

    // Upper bound of the playable area
    private float roofY = 4.2f;
    // Lower bound of the playable area
    private float groundY = -3.7f;

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

        // Clamp vertical position to playable area bounds
        float clampedY = Mathf.Clamp(transform.position.y, groundY, roofY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);

        // Kill upward velocity if at roof
        if (transform.position.y >= roofY && playerRb.linearVelocity.y > 0)
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
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
        if (collision.CompareTag("Hazard"))
        {
            LifeManager.Instance.LoseLife();
        }
    }
}
