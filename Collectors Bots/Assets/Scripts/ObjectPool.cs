using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _prefab;
    [SerializeField] private LevelCleaner _cleaner;

    private Queue<Transform> _pool;

    public IEnumerable<Component> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new Queue<Transform>();
    }

    public Component GetObject(Transform spawnPointTransform)
    {
        if(_pool.Count == 0)
        {
            var component = Instantiate(_prefab, spawnPointTransform.position, Quaternion.identity);
            component.transform.parent = _container;

            return component;
        }

        return _pool.Dequeue();
    }

    public void PutObject(Transform component)
    {
        _pool.Enqueue(component);
        component.gameObject.SetActive(false);
    }

    public void ReturnAllObjects()
    {
        _cleaner.CleanLevel();
    }
}
