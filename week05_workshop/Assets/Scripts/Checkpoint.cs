using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool _disableOnTrigger = false;

    private Collider _collider = null;

    public void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckpointManager.Instance.SaveCheckPoint(this);
        if (_disableOnTrigger)
        {
            this.enabled = false;
            _collider.enabled = false;
            transform.localScale = Vector3.one * 0.5f;
        }
    }
}
