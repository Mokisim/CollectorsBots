using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private float _spawnRate = 120;
    [SerializeField]private List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField]private int _spawnCount = 6;

    private WaitForSeconds _cooldown;

    private bool _isActive = true;

    private Coroutine _coroutine;

    private void Awake()
    {
        _cooldown = new WaitForSeconds(_spawnRate);
    }

    private void Start()
    {
        _coroutine = StartCoroutine(SpawnRecources());
    }

    private IEnumerator SpawnRecources()
    {
        while (_isActive == true)
        {
            Spawn();

            yield return _cooldown;
        }
    }

    private void Spawn()
    {
        List<int> randomSpawnpoints = new List<int>();
        int randomNumber;

        for(int i = 0; i < _spawnCount; i++)
        {
            do
            {
                randomNumber = Random.Range(0, _spawnPoints.Count);
            }
            while(randomSpawnpoints.Contains(randomNumber));

            randomSpawnpoints.Add(randomNumber);
        }

        for (int i = 0; i < _spawnCount; i++)
        {
            _pool.GetObject(_spawnPoints[randomSpawnpoints[i]]);
        }
    }
}
