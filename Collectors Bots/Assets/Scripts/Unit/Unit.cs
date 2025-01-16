using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private TakePoint _takePoint;
    [SerializeField] private Transform _baseTransform;

    public event Action<Unit> ArrivedBase;

    private Rigidbody _rigidbody;
    private int _resourceIndex = 1;
    private int _baseIndex = 2;
    private int _stayIndex = 0;
    private int _gettedIndex;

    private bool _isResourceReached => IsResourceReached;
    private Resource _resource => Resource;

    public Resource Resource { get; set; }
    public bool IsResourceReached { get; set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Resource != null && _isResourceReached == false)
        {
            GoTarget(_resourceIndex);
        }
        else if (_gettedIndex != _stayIndex)
        {
            GoTarget(_gettedIndex);
        }
    }

    private void OnEnable()
    {
        _takePoint.TargetReached += GoTarget;
    }

    private void OnDisable()
    {
        _takePoint.TargetReached -= GoTarget;
    }

    private void GoTarget(int targetIndex)
    {
        _gettedIndex = targetIndex;

        if (_gettedIndex == _stayIndex)
        {
            ArrivedBase.Invoke(this);
        }

        switch (targetIndex)
        {
            case var value when value == _stayIndex:
                Resource = null;
                return;

            case var value when value == _resourceIndex:
                transform.position = Vector3.MoveTowards(transform.position, _resource.transform.position, _speed * Time.deltaTime);
                LookAtTarget(_resource.transform);
                break;

            case var value when value == _baseIndex:
                IsResourceReached = true;
                transform.position = Vector3.MoveTowards(transform.position, _baseTransform.position, _speed * Time.deltaTime);
                LookAtTarget(_baseTransform);
                break;
        }
    }

    private void LookAtTarget(Transform target)
    {
        transform.LookAt(target.position);
    }
}
