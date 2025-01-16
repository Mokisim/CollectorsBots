using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private Transform _takePointTransform;

    private void Update()
    {
        if (_takePointTransform != null)
        {
            transform.position = _takePointTransform.position;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out TakePoint takePoint) == true && takePoint.Unit.Resource == this)
        {
            _takePointTransform = takePoint.transform;
        }
    }

    private void OnDisable()
    {
        _takePointTransform = null;
    }
}
