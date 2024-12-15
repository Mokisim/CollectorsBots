using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TakePoint : MonoBehaviour
{
    public event Action<int> TargetReached;

    [SerializeField] private Unit _unit;

    private ObjectPool _pool;
    private int _stayIndex = 0;
    private int _baseIndex = 2;
    private Resource _resource;

    private void Awake()
    {
        _pool = FindObjectOfType<ObjectPool>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource) == true && resource.IsTacked == false)
        {
            _resource = resource;
            TargetReached.Invoke(_baseIndex);
        }
        else if (collision.TryGetComponent(out Base @base) == true)
        {
            if (_resource != null)
            {
                TargetReached?.Invoke(_stayIndex);
            }

            if (_resource != null)
            {
                _pool.PutObject(_resource.transform);
                _resource = null;
            }
        }
    }
}
