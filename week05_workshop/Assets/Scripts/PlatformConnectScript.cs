using UnityEngine;

public class PlatformConnect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject != null)
        {
            other.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject != null)
        {
            other.transform.parent = null;
        }
    }
}
