using System;
using UnityEngine;

[Serializable] public class GameData
{
    public Vector3 LastCheckpointLocation = Vector3.zero;
    public float BestTime = 0.0f;
    public int GamePlayed = 0;
}
