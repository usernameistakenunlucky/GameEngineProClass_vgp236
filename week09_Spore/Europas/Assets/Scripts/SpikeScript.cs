using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Creature target = collision.GetComponentInParent<Creature>();

        if (target != null && target.gameObject != transform.root.gameObject) // stop creatures from comitting self harm
        {
            target.TakeDamage(_damage);
        }
    }
}