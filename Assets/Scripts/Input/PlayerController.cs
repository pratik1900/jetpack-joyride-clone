using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D playerRb;

    [SerializeField]
    private PlayerPowerupController powerupController;

    [Header("Movement")]
    [SerializeField]
    private float thrustSpeed = 5f;

    // [SerializeField] private float fallSpeed = 8f;

    // Upper bound of the playable area
    private float roofY = 4.2f;

    // Lower bound of the playable area
    private float groundY = -3.7f;

    // For Player Flash after getting hit
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private int flashCount = 6;

    [SerializeField]
    private float flashDuration = 0.08f;

    private InputAction thrustAction;

    private bool isInvincible = false;

    void Awake()
    {
        playerControls = new PlayerControls();
        playerRb = GetComponent<Rigidbody2D>();

        thrustAction = playerControls.Player.Thrust;

        if (powerupController == null)
        {
            powerupController = GetComponentInChildren<PlayerPowerupController>();
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard"))
        {
            Debug.Log("Check 1");
            ProcessHazardHit(collision);
        }
    }

    public void ProcessHazardHit(Collider2D collision)
    {
        if (
            powerupController != null
            && powerupController.Shield != null
            && powerupController.Shield.TryBlockHit()
        )
        {
            // collision.gameObject.GetComponent<Laser>().ReturnToPool();
            StartCoroutine(StartIFrames(1.5f));
            return;
        }

        // Checking IFrames
        if (isInvincible)
            return;
        GameEvents.TriggerPlayerHit();
        PlayerFlashAfterHit();
    }

    private void PlayerFlashAfterHit()
    {
        StartCoroutine(PlayerFlashRoutine());
    }

    private IEnumerator PlayerFlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private IEnumerator StartIFrames(float duration)
    {
        isInvincible = true;

        yield return new WaitForSeconds(duration);

        isInvincible = false;
    }
}
