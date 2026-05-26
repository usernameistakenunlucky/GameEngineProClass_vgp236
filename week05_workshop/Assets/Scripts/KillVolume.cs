using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other != null && gameObject != null)
        {
            PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
            if (player != null )
            {
                player.Respawn();
            }
        }
    }
}
