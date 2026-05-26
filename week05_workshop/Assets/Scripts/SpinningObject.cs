using UnityEngine;

public class SpinningObject : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 50.0f;

    void FixedUpdate()
    {
        transform.Rotate(0.0f, _spinSpeed * Time.fixedDeltaTime, 0.0f);
    }
}