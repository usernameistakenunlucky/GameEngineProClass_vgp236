using UnityEngine;

public class SpinningScript : MonoBehaviour
{
    [SerializeField]
    private float _spinSpeed = 90f;
    [SerializeField]
    private bool _direction = true;

    void Update()
    {
        int directionChange = 1;
        if (_direction) directionChange = -1;

        transform.Rotate(0f, 0f, _spinSpeed * Time.deltaTime * directionChange);
    }
}
