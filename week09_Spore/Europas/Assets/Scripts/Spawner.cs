using System.Collections.Generic;
using UnityEngine;

// spawn food and creatures randomly around the player
public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spawnPrefabs; // manually set what things to spawn, put like 3 plants so they spawn more often
    [SerializeField] private Transform _player;
    [SerializeField] private float _spawnRange = 20f;
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private int _maxObjects = 50;

    private float _timer;

    void Update()
    {
        if (_player == null) { return; }

        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            if (GameObject.FindGameObjectsWithTag("SpawnedObject").Length < _maxObjects) // cap objects, no more lagging out
            {
                SpawnObject();
            }
            _timer = 0f;
        }
    }

    private void SpawnObject()
    {
        if (_player == null) { return; }

        Vector2 randomPos = (Vector2)_player.position + new Vector2(Random.Range(-_spawnRange, _spawnRange), Random.Range(-_spawnRange, _spawnRange));

        if (Vector2.Distance(randomPos, _player.position) > 10f) // spawn them 10 grid spaces away minimum
        {
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            GameObject newObj = Instantiate(_spawnPrefabs[Random.Range(0, _spawnPrefabs.Count)], randomPos, randomRotation); // random rot
            newObj.tag = "SpawnedObject";

            Creature creature = newObj.GetComponent<Creature>();
            if (creature != null)
            {
                creature.Initialize(_player);
            }
        }
    }
}