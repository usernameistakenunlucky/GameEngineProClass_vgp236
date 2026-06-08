using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager
{
    private static GameData _gamedata = null;

    private static bool IsInitialized() => _gamedata != null;

    public static void Initialize()
    {
        if (_gamedata == null)
        {
            _gamedata = new GameData();
            Checkpoint checkpoint = CheckpointManager.Instance.GetSavedCheckPoint();
            if (checkpoint != null )
            {
                _gamedata.LastCheckpointLocation = checkpoint.transform.position;
            }
        }
    }

    private static string GetFullFilePath() => Application.dataPath + "/game.json";

    public static void Save()
    {
        string data = JsonUtility.ToJson( _gamedata, true );
        File.WriteAllText( GetFullFilePath(), data );
    }

    public static void Load()
    {
        string fullFilePath = GetFullFilePath();
        if (File.Exists(fullFilePath))
        {
            string data = File.ReadAllText(fullFilePath);
            _gamedata = JsonUtility.FromJson<GameData>(data); 
        }
    }

    public static GameData GetGameData() => _gamedata;

    public static void StoreBestTime(float time)
    {
        if(_gamedata.BestTime > 0.0f && time < _gamedata.BestTime)
        {
            _gamedata.BestTime = time;
        }
    }

    public static void IncrementGamesPlayed(int count = 1)
    {
        _gamedata.GamePlayed += count;
    }

    public static void SetLastSavedCheckpoint(Vector3 position)
    {
        _gamedata.LastCheckpointLocation = position;
    }
}
