using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance = null;
    public static GameController Instance => _instance;
    private float _gameTime = 0.0f;

    //private int _livesRemaining = 3;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        SaveManager.Initialize();
        SaveManager.Load();

        SaveManager.IncrementGamesPlayed();
        _gameTime = 0.0f;
    }

    private void Update()
    {
        _gameTime += Time.deltaTime;
    }

    public void GameOver()
    {


        //SaveManager.StoreBestTime(_gameTime);
        //SaveManager.Save();
    }    
}
