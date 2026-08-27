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

    float maxSpeed = 10f;

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

    // ****************************************
    [Header("Gravity Feel")]
    [SerializeField]
    private float baseGravityScale = 1f;

    [SerializeField]
    private float fallGravityMultiplier = 2.2f;

    [SerializeField]
    private float maxFallSpeed = -14f;

    [SerializeField]
    private float maxRiseSpeed = 8f;

    // ****************************************

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

    // void FixedUpdate()
    // {
    //     if (thrustAction.IsPressed())
    //     {
    //         playerRb.AddForce(Vector2.up * thrustSpeed, ForceMode2D.Force);
    //     }

    //     // Clamp velocity instead of accelerating forever (optional but common)
    //     playerRb.linearVelocity = new Vector2(
    //         playerRb.linearVelocity.x,
    //         Mathf.Clamp(playerRb.linearVelocity.y, -maxSpeed, maxSpeed)
    //     );

    //     // Clamp position using the Rigidbody, not transform directly
    //     float clampedY = Mathf.Clamp(playerRb.position.y, groundY, roofY);
    //     if (!Mathf.Approximately(clampedY, playerRb.position.y))
    //     {
    //         playerRb.position = new Vector2(playerRb.position.x, clampedY);

    //         // Kill velocity in whichever direction we just clamped
    //         if (playerRb.position.y >= roofY && playerRb.linearVelocity.y > 0)
    //             playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
    //         if (playerRb.position.y <= groundY && playerRb.linearVelocity.y < 0)
    //             playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
    //     }
    // }

    void FixedUpdate()
    {
        if (thrustAction.IsPressed())
        {
            playerRb.AddForce(Vector2.up * thrustSpeed, ForceMode2D.Force);
            playerRb.gravityScale = baseGravityScale;
        }
        else if (playerRb.linearVelocity.y < 0)
        {
            // Falling and not thrusting — apply extra gravity
            playerRb.gravityScale = baseGravityScale * fallGravityMultiplier;
        }
        else
        {
            playerRb.gravityScale = baseGravityScale;
        }

        // Asymmetric clamp instead of a single maxSpeed
        playerRb.linearVelocity = new Vector2(
            playerRb.linearVelocity.x,
            Mathf.Clamp(playerRb.linearVelocity.y, maxFallSpeed, maxRiseSpeed)
        );

        // Clamp position using the Rigidbody, not transform directly
        float clampedY = Mathf.Clamp(playerRb.position.y, groundY, roofY);
        if (!Mathf.Approximately(clampedY, playerRb.position.y))
        {
            playerRb.position = new Vector2(playerRb.position.x, clampedY);

            // Kill velocity in whichever direction we just clamped
            if (playerRb.position.y >= roofY && playerRb.linearVelocity.y > 0)
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
            if (playerRb.position.y <= groundY && playerRb.linearVelocity.y < 0)
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard"))
        {
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
        StartCoroutine(StartIFrames(1.5f)); // to prevent multiple hits in a short time frame
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
