using UnityEngine;

public class KillItemScript : MonoBehaviour
{
    [SerializeField]
    private int _damage = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject != null)
        {
            Destroy(collision.gameObject, 1.5f); //second parm is the delay

            GameManager.Instance.DamagePlayer(_damage);
            GameManager.Instance.UpdatePlayer();

            GameManager.Instance.DamageBoss(GameManager.Instance.GetScore());
            GameManager.Instance.UpdateBoss();

            GameManager.Instance.ResetDamage();
            GameManager.Instance.UpdateDamage();
        }
    }
}
