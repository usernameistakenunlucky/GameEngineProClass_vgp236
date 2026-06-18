using UnityEngine;

// agressive chasing creature, hard to kill

public class Creature_Spikey : Creature
{
    [Header("Spikey Creature Parms -----------------------------------------------------")]
    //[SerializeField] protected Transform _playerTransform;
    [SerializeField] private float _detectionRadius = 4f;
    [SerializeField] private float _followRadius = 8f;
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _enemyRotationSpeed = 180f;

    [Header("Drops -------------------------------------------------------------------")]
    [SerializeField] private GameObject _spikePartPrefab;
    [SerializeField][Range(0f, 1f)] private float _dropChance = 0.25f;

    private Rigidbody2D _rigidBody;
    private bool _isChasing;

    protected override void Start()
    {
        base.Start();
        _meatDropCount = 3;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, _playerTransform.position);

            if (!_isChasing)
            {
                if (distance <= _detectionRadius)
                {
                    _isChasing = true;
                }
            }
            else
            {
                if (distance > _followRadius)
                {
                    _isChasing = false;
                }
            }

            if (_isChasing)
            {
                ChasePlayer();
            }
            else
            {
                _rigidBody.linearVelocity = Vector2.MoveTowards(_rigidBody.linearVelocity, Vector2.zero, _acceleration * Time.fixedDeltaTime);
            }
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        Vector2 targetVelocity = direction * _moveSpeed;

        _rigidBody.linearVelocity = Vector2.MoveTowards(_rigidBody.linearVelocity, targetVelocity, _acceleration * Time.fixedDeltaTime);

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _enemyRotationSpeed * Time.fixedDeltaTime);
    }

    protected override void Die()
    {
        if (_spikePartPrefab != null && Random.value <= _dropChance) // chance to give spikeys
        {
            Instantiate(_spikePartPrefab, transform.position, Quaternion.identity);
        }

        base.Die();
    }
}