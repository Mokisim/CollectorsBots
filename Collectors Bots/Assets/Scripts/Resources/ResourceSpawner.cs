using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private float _spawnRate = 120;
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField] private int _spawnCount = 6;

    private WaitForSeconds _cooldown;

    private bool _isActive = true;

    public event Action Spawned;

    private void Awake()
    {
        _cooldown = new WaitForSeconds(_spawnRate);
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnRecources());
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnRecources());
    }

    private IEnumerator SpawnRecources()
    {
        while (_isActive == true)
        {
            Spawned?.Invoke();
            Spawn();

            yield return _cooldown;
        }
    }

    private void Spawn()
    {
        List<int> randomSpawnpoints = new List<int>();
        int randomNumber;

        for (int i = 0; i < _spawnCount; i++)
        {
            do
            {
                randomNumber = UnityEngine.Random.Range(0, _spawnPoints.Count);
            }
            while (randomSpawnpoints.Contains(randomNumber));

            randomSpawnpoints.Add(randomNumber);
        }

        for (int i = 0; i < _spawnCount; i++)
        {
            var resource = _pool.GetObject();

            resource.gameObject.SetActive(true);
            resource.transform.position = _spawnPoints[randomSpawnpoints[i]].position;
        }
    }
}
