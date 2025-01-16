using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPointsContainer;
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private float _spawnRate = 120;

    private List<Transform> _spawnPoints = new List<Transform>();
    private WaitForSeconds _cooldown;
    private int _spawnCount = 6;

    private bool _isActive = true;

    private Coroutine _coroutine;

    private void Awake()
    {
        for (int i = 0; i < _spawnPointsContainer.childCount; i++)
        {
            _spawnPoints.Add(_spawnPointsContainer.GetChild(i));
        }

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
