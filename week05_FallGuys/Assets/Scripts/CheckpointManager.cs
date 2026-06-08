using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    static public CheckpointManager _instance = null;
    static public CheckpointManager Instance => _instance;

    private Checkpoint _lastSavedCheckpoint = null;

    private void Awake()
    {
        if(_instance == null )
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCheckPoint(Checkpoint checkpoint)
    {
        _lastSavedCheckpoint = checkpoint;
    }

    public Checkpoint GetSavedCheckPoint()
    {
        return _lastSavedCheckpoint;
    }
}
