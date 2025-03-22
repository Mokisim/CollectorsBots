using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsPool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    
    private Queue<Transform> _pool;

    public IEnumerable<Component> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new Queue<Transform>();
    }

    public Component GetObject(Transform prefab)
    {
        if (_pool.Count == 0)
        {
            var component = Instantiate(prefab, Vector3.zero, transform.rotation);
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
