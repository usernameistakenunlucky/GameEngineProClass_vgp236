using UnityEngine;

public class DoubleScoreScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject != null && collision.tag == "DropItem")
        {
            GameManager.Instance.AddScore(GameManager.Instance.GetScore());
        }
    }
}
