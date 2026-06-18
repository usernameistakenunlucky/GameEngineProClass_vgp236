using UnityEngine;

// scaredy boy

public class Creature_Speed : Creature
{
    [Header("Speed Creature Parms ------------------------------------------------------")]
    //[SerializeField] protected Transform _playerTransform;
    [SerializeField] private float _detectionRadius = 4f;
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _enemyRotationSpeed = 300f;
    [SerializeField] private float _burstInterval = 3f;
    [SerializeField] private float _burstDuration = 0.5f;

    [Header("Drops -------------------------------------------------------------------")]
    [SerializeField] private GameObject _finPartPrefab;
    [SerializeField][Range(0f, 1f)] private float _dropChance = 0.25f;

    private Rigidbody2D _rigidBody;
    private float _burstTimer;
    private bool _isBursting;

    protected override void Start()
    {
        base.Start();
        _meatDropCount = 2;
        _maxHealth = 75; // lower hp than big guys
        _rigidBody = GetComponent<Rigidbody2D>();
        _burstTimer = Random.Range(0f, _burstInterval);
    }

    void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, _playerTransform.position);
            _burstTimer += Time.fixedDeltaTime;

            if (distance <= _detectionRadius)
            {
                if (_burstTimer >= _burstInterval && !_isBursting)
                {
                    _isBursting = true;
                    _burstTimer = 0f;
                }

                if (_isBursting)
                {
                    if (_burstTimer < _burstDuration)
                    {
                        FleeFromPlayer();
                    }
                    else
                    {
                        _isBursting = false;
                        _burstTimer = 0f;
                    }
                }
                else
                {
                    _rigidBody.linearVelocity = Vector2.MoveTowards(_rigidBody.linearVelocity, Vector2.zero, _acceleration * Time.fixedDeltaTime);
                }
            }
            else
            {
                _isBursting = false;
                _rigidBody.linearVelocity = Vector2.MoveTowards(_rigidBody.linearVelocity, Vector2.zero, _acceleration * Time.fixedDeltaTime);
            }
        }
    }

    void FleeFromPlayer() // ai take the wheel~
    {
        Vector2 directionAway = (transform.position - _playerTransform.position).normalized;
        Vector2 targetVelocity = directionAway * (_moveSpeed * 2.5f);

        _rigidBody.linearVelocity = Vector2.MoveTowards(_rigidBody.linearVelocity, targetVelocity, _acceleration * Time.fixedDeltaTime);

        float targetAngle = Mathf.Atan2(directionAway.y, directionAway.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _enemyRotationSpeed * Time.fixedDeltaTime);
    }

    protected override void Die()
    {
        if (_finPartPrefab != null && Random.value <= _dropChance) // chance to drop the fins
        {
            Instantiate(_finPartPrefab, transform.position, Quaternion.identity);
        }

        base.Die();
    }
}