using UnityEngine;
using TMPro;

public enum PartType
{
    Spikes,
    Fins,
}

// for unlocking spikes and fins
public class PartPickup : MonoBehaviour
{
    [SerializeField] private PartType _partType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.EquipPart(_partType);
            Destroy(gameObject);
        }
    }
}