using UnityEngine;

public class PegScript : MonoBehaviour
{
    [SerializeField]
    private float _bounceBoost = 1.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DropItem")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity *= _bounceBoost;
            }
        }
    }
}