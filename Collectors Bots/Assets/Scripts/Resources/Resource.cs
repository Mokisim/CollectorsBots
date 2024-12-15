using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private Transform _takePointTransform;
    private bool _isTacked = false;
    private bool _isAssigned => IsAssigned;

    public bool IsTacked => _isTacked;
    public bool IsAssigned;

    private void Awake()
    {
        IsAssigned = false;
    }

    private void Update()
    {
        if (_takePointTransform != null)
        {
            transform.position = _takePointTransform.position;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out TakePoint takePoint) == true)
        {
            _takePointTransform = takePoint.transform;
            _isTacked = true;
        }
    }

    private void OnDisable()
    {
        _takePointTransform = null;
        _isTacked = false;
        IsAssigned = false;
    }
}
