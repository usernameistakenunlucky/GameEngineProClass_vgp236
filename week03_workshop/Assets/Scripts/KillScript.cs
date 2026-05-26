using UnityEngine;

public class KillItemScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject != null)
        {
            //Destroy(collision.gameObject, 1.5f); //second parm is the delay

            //respawn player

        }
    }
}
