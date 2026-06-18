using UnityEngine;

// damage dealing logic for any mouth enjoyers

public class MouthScript : MonoBehaviour
{
    [SerializeField] protected float _ramDamage = 25f;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Creature target = collision.GetComponentInParent<Creature>();

        if (target != null)
        {
            target.TakeDamage(_ramDamage);
        }
    }
}
