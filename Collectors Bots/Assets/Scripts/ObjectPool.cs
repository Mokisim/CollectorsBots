using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _prefab;
    [SerializeField] private List<Unit> _allUnits;

    private Queue<Transform> _pool;
    
    public IEnumerable<Component> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new Queue<Transform>();
    }

    private void OnEnable()
    {
        foreach(Unit unit in _allUnits)
        {
            unit.TakePoint.ResourceReturned += PutObject;
        }
    }

    private void OnDisable()
    {
        foreach (Unit unit in _allUnits)
        {
            unit.TakePoint.ResourceReturned -= PutObject;
        }
    }

    public void AddUnit(Unit unit)
    {
        _allUnits.Add(unit);
        unit.TakePoint.ResourceReturned += PutObject;
    }

    public Component GetObject()
    {
        if (_pool.Count == 0)
        {
            var component = Instantiate(_prefab, Vector3.zero, transform.rotation);
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
