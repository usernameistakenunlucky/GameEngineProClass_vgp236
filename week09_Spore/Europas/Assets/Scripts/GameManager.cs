using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// for game over handling / respawn
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI _deathText;

    private bool _isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (_deathText != null) _deathText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R)) // restart game if dead dead
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameOver() // DIED
    {
        _isGameOver = true;
        if (_deathText != null)
        {
            _deathText.gameObject.SetActive(true);
            _deathText.text = "YOU DIED";
        }
    }
}