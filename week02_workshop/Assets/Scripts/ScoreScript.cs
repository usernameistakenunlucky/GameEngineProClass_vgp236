using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    [SerializeField]
    private int _score = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.gameObject != null && collision.tag == "DropItem")
        {
            GameManager.Instance.AddScore(_score);
            Destroy(collision.gameObject);
        }
    }
}
