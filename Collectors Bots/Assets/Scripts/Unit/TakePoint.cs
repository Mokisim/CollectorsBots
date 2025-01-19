using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TakePoint : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private ObjectPool _pool;

    private int _stayIndex = 0;
    private int _baseIndex = 2;
    private Resource _resource;

    public event Action<int> TargetReached;

    public Unit Unit => _unit;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource) == true && _unit.Resource == resource)
        {
            _resource = resource;
            _resource.Take(this.transform);
            TargetReached.Invoke(_baseIndex);
        }
        else if (collision.TryGetComponent(out Base @base) == true && _unit.Resource != null)
        {
            if (_resource != null)
            {
                TargetReached?.Invoke(_stayIndex);
                _resource.Put();
                _pool.PutObject(_resource.transform);
                _resource = null;
            }
        }
    }
}
