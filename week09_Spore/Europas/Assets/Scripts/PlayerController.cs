using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MISC ----------------------------------------------------------------------")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 200f;
    [SerializeField] private float _acceleration = 10f;

    [Header("Visuals -------------------------------------------------------------------")]
    [SerializeField] private GameObject _spikesVisual;
    [SerializeField] private GameObject _finsVisual;

    [Header("Dash Settings -------------------------------------------------------------")]
    [SerializeField] private float _dashSpeedMultiplier = 3.5f;
    [SerializeField] private float _dashDuration = 0.25f;
    [SerializeField] private float _dashCooldown = 5f;

    private bool _hasSpikes = false; // will activate with drops
    private bool _hasFins = false;

    private Rigidbody2D _rigidBody;
    private Vector2 _moveInput;
    private Vector2 _dashDirection;

    private float _dashCooldownTimer = 0f;
    private float _dashDurationTimer = 0f;
    private bool _isDashing = false;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        if (_spikesVisual != null) _spikesVisual.SetActive(false);
        if (_finsVisual != null) _finsVisual.SetActive(false);
    }

    void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }

        if (_hasFins && Input.GetKeyDown(KeyCode.Space) && _dashCooldownTimer <= 0f && !_isDashing)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        if (_isDashing)
        {
            PerformDash();
        }
        else
        {
            MovePlayer();
        }
    }

    void StartDash()
    {
        _isDashing = true;
        _dashDurationTimer = _dashDuration;
        _dashCooldownTimer = _dashCooldown;

        if (_moveInput == Vector2.zero)
        {
            _dashDirection = transform.up;
        }
        else
        {
            _dashDirection = _moveInput;
        }
    }

    void PerformDash()
    {
        _dashDurationTimer -= Time.fixedDeltaTime;
        _rigidBody.linearVelocity = _dashDirection * (_moveSpeed * _dashSpeedMultiplier);

        if (_dashDurationTimer <= 0f)
        {
            _isDashing = false;
        }
    }

    void MovePlayer()
    {
        Vector2 targetVelocity = _moveInput * _moveSpeed;
        _rigidBody.linearVelocity = Vector2.MoveTowards(_rigidBody.linearVelocity, targetVelocity, _acceleration * Time.fixedDeltaTime);

        if (_moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = _rigidBody.rotation;
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
            _rigidBody.angularVelocity = angleDifference * (_turnSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _rigidBody.angularVelocity = Mathf.MoveTowards(_rigidBody.angularVelocity, 0f, _turnSpeed * Time.fixedDeltaTime);
        }
    }

    public void IncreaseSpeed(float multiplier)
    {
        _moveSpeed *= multiplier;
    }

    public void EquipPart(PartType partType)
    {
        if (partType == PartType.Spikes)
        {
            _hasSpikes = true;
            if (_spikesVisual != null)
            {
                _spikesVisual.SetActive(true);
            }
        }
        else if (partType == PartType.Fins)
        {
            _hasFins = true;
            if (_finsVisual != null)
            {
                _finsVisual.SetActive(true);
            }
        }
    }
}