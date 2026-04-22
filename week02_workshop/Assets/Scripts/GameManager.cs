using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText = null;
    private int _currentScore = 0;


    private static GameManager instance = null;

    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            UpdateScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        _currentScore += amount;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _scoreText.text = "Score: " + _currentScore.ToString();
    }

}
