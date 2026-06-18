using UnityEngine;

// jut for the hp bar to stop rotating
public class UIKeepUpright : MonoBehaviour
{
    private Quaternion _initialRotation;

    private void Start()
    {
        _initialRotation = Quaternion.identity;
    }

    private void LateUpdate()
    {
        transform.rotation = _initialRotation;
    }
}