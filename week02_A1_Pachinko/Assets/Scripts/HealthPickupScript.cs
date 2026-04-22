using UnityEngine;

public class HealthPickupScript : MonoBehaviour
{
    [SerializeField]
    private int _heal = 150;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject != null && collision.tag == "DropItem")
        {
            GameManager.Instance.HealPlayer(_heal);
            GameManager.Instance.UpdatePlayer();
            Destroy(this);
        }
    }
}
