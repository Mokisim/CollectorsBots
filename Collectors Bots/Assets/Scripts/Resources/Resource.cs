using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private Transform _takePointTransform;

    private void Update()
    {
        if(_takePointTransform != null)
        {
            transform.position = _takePointTransform.position;
        }
    }

    public void Take(Transform takePoint)
    {
        _takePointTransform = takePoint;
    }

    public void Put()
    {
        _takePointTransform = null;
    } 
}
