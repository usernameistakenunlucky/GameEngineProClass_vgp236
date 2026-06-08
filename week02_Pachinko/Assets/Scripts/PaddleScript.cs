using UnityEngine;
public class PaddleScript : MonoBehaviour
{
    [SerializeField]
    private float _flipAngle = 60f;
    [SerializeField]
    private float _flipSpeed = 10f;
    [SerializeField]
    private bool _direction = true;

    private Rigidbody2D _rb;
    private float _restAngle;
    private float _flippedAngle;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _restAngle = transform.eulerAngles.z;
        float angle = _direction ? _flipAngle : -_flipAngle;
        _flippedAngle = _restAngle + angle;
    }

    void Update()
    {
        float targetAngle = Input.GetKey(KeyCode.E) ? _flippedAngle : _restAngle;
        float newAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, _flipSpeed * Time.deltaTime);
        _rb.MoveRotation(newAngle);
    }
}