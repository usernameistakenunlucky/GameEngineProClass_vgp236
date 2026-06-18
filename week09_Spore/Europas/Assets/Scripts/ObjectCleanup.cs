using UnityEngine;

// kill objects that get too far from player so i dont get 7 trillion creatures on map
public class ObjectCleanup : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 50f;
    private Transform _playerTransform;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); 
        if (playerObj != null)
        {
            _playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            if (Vector2.Distance(transform.position, _playerTransform.position) > _maxDistance) // kill it when out of range
            {
                Destroy(gameObject);
            }
        }
    }
}