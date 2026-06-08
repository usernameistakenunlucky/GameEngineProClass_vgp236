using UnityEngine;

public class EndGoalScript : MonoBehaviour
{
    [SerializeField] private PlayerScript _player = null;
    private bool _isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered || !_player.HasGoldCross) // if player doesnt have the cross, ignore him
        {
            return;
        }

        _isTriggered = true;

        _player.DisableControl();
    }
}