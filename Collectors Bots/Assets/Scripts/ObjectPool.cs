using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _prefab;
    [SerializeField] private List<TakePoint> _takePoints;

    private Queue<Transform> _pool;
    
    public IEnumerable<Component> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new Queue<Transform>();
    }

    private void OnEnable()
    {
        foreach(TakePoint takePoint in _takePoints)
        {
            takePoint.ResourceReturned += PutObject;
        }
    }

    private void OnDisable()
    {
        foreach (TakePoint takePoint in _takePoints)
        {
            takePoint.ResourceReturned -= PutObject;
        }
    }

    public Component GetObject(Transform spawnPointTransform)
    {
        if (_pool.Count == 0)
        {
            var component = Instantiate(_prefab, spawnPointTransform.position, Quaternion.identity);
            component.transform.parent = _container;

            return component;
        }

        return _pool.Dequeue();
    }

    public void PutObject(Transform component)
    {
        component.transform.parent = _container;
        _pool.Enqueue(component);
        component.gameObject.SetActive(false);
    }
}
