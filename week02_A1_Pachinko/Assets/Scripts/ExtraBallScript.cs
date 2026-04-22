using UnityEngine;

public class ExtraBallScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject != null && collision.tag == "DropItem")
        {
            GameManager.Instance.RegainBall();
            GameManager.Instance.UpdateBalls();
            Destroy(this);
        }
    }
}
