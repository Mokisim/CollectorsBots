using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TakePoint : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private ObjectPool _pool;

    public event Action<int> TargetReached;

    private int _stayIndex = 0;
    private int _baseIndex = 2;
    private Resource _resource;

    public Unit Unit
    {
        get 
        { 
            return _unit; 
        }
        private set { }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource) == true && _unit.Resource == resource)
        {
            _resource = resource;
            TargetReached.Invoke(_baseIndex);
        }
        else if (collision.TryGetComponent(out Base @base) == true)
        {
            if (_resource != null)
            {
                TargetReached?.Invoke(_stayIndex);
                _pool.PutObject(_resource.transform);
                _resource = null;
            }
        }
    }
}
