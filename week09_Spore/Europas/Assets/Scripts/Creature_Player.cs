using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum DietType
{
    Carnivore,
    Herbivore,
}

// holds info about the player's creature, hp / growth etc
public class PlayerCreature : Creature
{
    [SerializeField] private DietType _diet;

    [Header("Progression -------------------------------------------------------------------")]
    [SerializeField] private int _score = 0;
    [SerializeField] private int _scoreGoal = 100;
    [SerializeField] private int _pointsPerGrowth = 20;
    [SerializeField] private float _currentSize = 1f;

    [Header("UI Progression --------------------------------------------------------------")]
    [SerializeField] private Image _progressBarFill;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _winText; 

    private PlayerController _playerController;
    private int _scoreSinceLastGrowth = 0;

    public DietType GetDiet() => _diet;

    protected override void Start()
    {
        base.Start();
        _maxHealth = 120; // give player more hp
        _meatDropCount = 0;
        _playerController = GetComponent<PlayerController>();

        if (_winText != null)
        {
            _winText.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    public void AddScore(int points)
    {
        _score += points;
        _scoreSinceLastGrowth += points;
        UpdateUI();

        if (_score >= _scoreGoal)
        {
            WinGame();
            return;
        }

        if (_scoreSinceLastGrowth >= _pointsPerGrowth)
        {
            Grow();
        }
    }

    private void Grow()
    {
        _currentSize += 0.2f;
        transform.localScale = new Vector3(_currentSize, _currentSize, 1f);

        _scoreSinceLastGrowth = 0;

        _maxHealth *= 1.10f;

        UpdateMaxHealthUI(_maxHealth);

        if (_playerController != null)
        {
            _playerController.IncreaseSpeed(1.10f);
        }
    }

    private void WinGame()
    {
        if (_winText != null)
        {
            _winText.gameObject.SetActive(true);
            _winText.text = "You're very fat now,\nYOU WIN!";
        }
    }

    private void UpdateUI()
    {
        if (_progressBarFill != null)
        {
            float progress = (float)_score / (float)_scoreGoal;
            _progressBarFill.fillAmount = Mathf.Clamp01(progress);
        }

        if (_scoreText != null)
        {
            _scoreText.text = _score + " / " + _scoreGoal;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger detected on Player: " + collision.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected on Player: " + collision.gameObject.name);
    }
}