using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (SphereCollider))]
public class TakePoint : MonoBehaviour
{
    public event Action<int> TargetReached;

    [SerializeField] private Unit _unit;

    private int _stayIndex = 0;
    private int _resourceIndex = 1;
    private int _baseIndex = 2;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.TryGetComponent(out Resource resource) == true)
        {
            TargetReached.Invoke(_baseIndex);
        }
        else if(collision.TryGetComponent(out Base @base) == true)
        {
            TargetReached?.Invoke(_stayIndex);
        }
    }
}
