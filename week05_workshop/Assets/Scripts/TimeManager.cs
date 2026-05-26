using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float _timePassed = 0f;
    private bool _isStopped = false;

    public void StopTimer()
    {
        _isStopped = true;
    }

    void Update()
    {
        if (_isStopped)
        {
            return;
        }

        _timePassed += Time.deltaTime;
        DisplayTime(_timePassed);
    }

    void DisplayTime(float time)
    {
        float seconds = (time % 60);
        int minutes = (int)((time - seconds) / 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //                                 ^^^(I asked AI how to format it)
    }
}
