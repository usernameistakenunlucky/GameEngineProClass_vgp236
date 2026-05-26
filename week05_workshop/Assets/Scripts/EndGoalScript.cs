using UnityEngine;

public class EndGoalScript : MonoBehaviour
{
    [SerializeField] private Timer _timer = null;
    [SerializeField] private PlayerScript _player = null;
    private bool _isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered)
        {
            return;
        }

        _isTriggered = true;

        _player.DisableControl();
        _timer.StopTimer();

        GameController.Instance.GameOver();
    }
}