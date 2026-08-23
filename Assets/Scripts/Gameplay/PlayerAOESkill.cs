using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Hold the skill key to show a circle centered on the player.
/// Release it to detonate: every enemy inside the circle at that moment
/// gets its Explode() called via the IExplodable interface.
///
/// Setup:
///  1. Add this script to the Player GameObject.
///  2. Create a child GameObject "AoeIndicator" under the Player,
///     add a LineRenderer to it, and drag it into the "Indicator" field below.
///  3. Put your enemies on a dedicated "Enemy" layer and set that layer
///     in "Enemy Layer" below. Enemies need a Collider2D (can be a trigger).
///  4. Make sure each enemy script implements IExplodable.
/// </summary>
public class PlayerAoeSkill : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputActionReference AOESkillAction;

    [Header("Skill Settings")]
    [SerializeField]
    private float radius = 3f;

    [SerializeField]
    private LayerMask hazardLayer;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("Optional: charge-up (grows while held)")]
    [SerializeField]
    private bool growWhileHeld = false;

    [SerializeField]
    private float growSpeed = 500f;

    [SerializeField]
    private float maxRadius = 5f;

    [Header("Optional: cooldown")]
    [SerializeField]
    private float cooldown = 15f;
    private float _cooldownTimer;

    private bool _skillActive;
    private float _currentRadius;

    private float _baseSpriteRadius;

    // Reused buffer so the overlap query doesn't allocate garbage every cast
    private Collider2D[] _hitsBuffer = new Collider2D[32];

    void Start()
    {
        spriteRenderer.enabled = false;
        _baseSpriteRadius = spriteRenderer.sprite.bounds.extents.x; // half of bounds.size.x
    }

    private void OnEnable()
    {
        AOESkillAction.action.Enable();
    }

    private void OnDisable()
    {
        AOESkillAction.action.Disable();
    }

    private void Update()
    {
        if (_cooldownTimer > 0f)
            _cooldownTimer -= Time.deltaTime;

        if (AOESkillAction.action.WasPressedThisFrame() && _cooldownTimer <= 0f)
        {
            StartSkill();
        }

        if (_skillActive && growWhileHeld)
        {
            if (_currentRadius < maxRadius)
            {
                Debug.Log("AOE Skill growing");
                _currentRadius = Mathf.Min(_currentRadius + growSpeed * Time.deltaTime, maxRadius);
                SetIndicatorRadius(_currentRadius);
            }
            else
            {
                Debug.Log("AOE Skill reached max radius");
            }
            // Debug.Log("AOE Skill growing");
            // _currentRadius = Mathf.Min(_currentRadius + growSpeed * Time.deltaTime, maxRadius);
            // SetIndicatorRadius(_currentRadius);
        }

        if (_skillActive && AOESkillAction.action.WasReleasedThisFrame())
        {
            ReleaseSkill();
        }
    }

    private void StartSkill()
    {
        Debug.Log("AOE Skill activated");
        _skillActive = true;
        _currentRadius = growWhileHeld ? 0f : radius;
        SetIndicatorRadius(_currentRadius);
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }

    private void ReleaseSkill()
    {
        Debug.Log("AOE Skill released");
        if (!_skillActive)
            return;

        _skillActive = false;
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        _cooldownTimer = cooldown;

        float finalRadius = growWhileHeld ? _currentRadius : radius;

        _hitsBuffer = Physics2D.OverlapCircleAll(transform.position, finalRadius, hazardLayer);
        Debug.Log($"AOE Skill hit {_hitsBuffer.Length} objects");
        foreach (Collider2D hit in _hitsBuffer)
        {
            // if (_hitsBuffer[i].TryGetComponent<IExplodable>(out var explodable))
            if (hit.gameObject.TryGetComponent<IPooledObject>(out var explodable))
            {
                // explodable.Explode();
                explodable.ReturnToPool();
            }
        }
    }

    private void SetIndicatorRadius(float value)
    {
        if (spriteRenderer == null || _baseSpriteRadius <= 0f)
            return;

        float scaleFactor = value / _baseSpriteRadius;
        spriteRenderer.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }
}
