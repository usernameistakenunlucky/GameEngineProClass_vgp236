using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _damageText = null;
    private int _currentDamage = 0;

    [SerializeField]
    private TMP_Text _bossText = null;
    private int _bossHp = 1000;

    [SerializeField]
    private TMP_Text _playerText = null;
    private int _playerMaxHp = 300;
    private int _playerHp = 300;

    [SerializeField]
    private TMP_Text _ballsText = null;
    private int _ballsRemaining = 5;


    private static GameManager instance = null;

    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            UpdateDamage();
            UpdatePlayer();
            UpdateBoss();
            UpdateBalls();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetBallsRemaining() { return _ballsRemaining; }
    public int GetPlayerHp() { return _playerHp; }
    public void UseBall() { _ballsRemaining -= 1; }
    public void RegainBall() { _ballsRemaining += 1; }
    public int GetScore() { return _currentDamage; }
    public void ResetDamage() { _currentDamage = 0; }

    public void AddScore(int amount)
    {
        _currentDamage += amount;
        UpdateDamage();
    }

    public void DamagePlayer(int amount)
    {
        _playerHp -= amount;
        if (_playerHp < 0) { _playerHp = 0; }
    }

    public void HealPlayer(int amount)
    {
        _playerHp += amount;
        if (_playerHp > _playerMaxHp) { _playerHp = _playerMaxHp; }
    }

    public void DamageBoss(int amount)
    {
        _bossHp -= amount;
        if (_bossHp < 0 ) { _bossHp = 0; }
    }

    public void UpdateDamage()
    {
        _damageText.text = "Damage: " + _currentDamage.ToString();
    }

    public void UpdateBoss()
    {
        _bossText.text = "Boss Hp: " + _bossHp.ToString();
    }

    public void UpdatePlayer()
    {
        _playerText.text = "Player Hp: " + _playerHp.ToString();
    }

    public void UpdateBalls()
    {
        _ballsText.text = "Balls Remaining: " + _ballsRemaining.ToString();
    }

}
