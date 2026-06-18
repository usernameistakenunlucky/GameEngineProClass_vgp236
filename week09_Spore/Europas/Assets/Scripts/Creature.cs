using System.Collections;
using UnityEngine;
using UnityEngine.UI;


// used for player and other creatures, 

public class Creature : MonoBehaviour
{
    [Header("MISC ------------------------------------------------------")]
    [SerializeField] protected float _maxHealth = 100f;
    [SerializeField] protected GameObject _meatPrefab;
    [SerializeField] protected int _meatDropCount = 0;
    [SerializeField] private float _dropForce = 3f;

    [Header("Health Bar UI ---------------------------------------------")]
    [SerializeField] protected Slider _healthSlider;
    [SerializeField] private GameObject _healthBarCanvas;
    [SerializeField] private float _uiDisplayDuration = 5f;
    [SerializeField] private float _verticalOffset = 0f;

    protected float _currentHealth;
    private Coroutine _uiTimerCoroutine;
    private Transform _uiTransform;

    protected Transform _playerTransform;
    public void Initialize(Transform player)
    {
        _playerTransform = player;
    }

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;

        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _currentHealth;
        }

        if (_healthBarCanvas != null)
        {
            _uiTransform = _healthBarCanvas.transform;
            _uiTransform.SetParent(null);
            _healthBarCanvas.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (_healthBarCanvas != null && _healthBarCanvas.activeSelf)
        {
            // Lock position strictly to the creature's position plus a true vertical offset
            _uiTransform.position = transform.position + new Vector3(0f, _verticalOffset, 0f);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        if (_healthSlider != null)
        {
            _healthSlider.value = _currentHealth;
        }

        if (_healthBarCanvas != null)
        {
            if (_uiTimerCoroutine != null)
            {
                StopCoroutine(_uiTimerCoroutine);
            }
            _uiTimerCoroutine = StartCoroutine(HandleHealthBarVisibility());
        }

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private IEnumerator HandleHealthBarVisibility()
    {
        _healthBarCanvas.SetActive(true);
        yield return new WaitForSeconds(_uiDisplayDuration);
        _healthBarCanvas.SetActive(false);
    }

    protected virtual void Die()
    {
        if (CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }

        if (_healthBarCanvas != null)
        {
            Destroy(_healthBarCanvas);
        }

        SpawnFoodDrops();
        Destroy(gameObject);
    }

    private void SpawnFoodDrops()
    {
        if (_meatPrefab != null)
        {
            for (int i = 0; i < _meatDropCount; i++)
            {
                GameObject foodObj = Instantiate(_meatPrefab, transform.position, Quaternion.identity);
                ApplyRandomVelocity(foodObj);
            }
        }
    }

    public void UpdateMaxHealthUI(float newMaxHealth)
    {
        if (_healthSlider != null)
        {
            _healthSlider.maxValue = newMaxHealth;
        }
    }

    private void ApplyRandomVelocity(GameObject foodObj)
    {
        Rigidbody2D rb = foodObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDirection * _dropForce, ForceMode2D.Impulse);
        }
    }
}