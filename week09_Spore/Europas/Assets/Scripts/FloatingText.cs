using UnityEngine;

// for when food is eaten, see floating text number
public class FloatingText : MonoBehaviour
{
    [SerializeField] private float _destroyDelay = 2f;

    private void Start()
    {
        Destroy(gameObject, _destroyDelay);
    }
}