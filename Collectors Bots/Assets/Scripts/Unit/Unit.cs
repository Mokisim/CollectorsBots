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

    private bool _isResourceReached;

    private Resource _resource;

    public Resource Resource => _resource;
    
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

    public void SetReached()
    {
        _isResourceReached = true;
    }

    public void SetUnreached()
    {
        _isResourceReached = false;
    }

    public void ClearResource()
    {
        _resource = null;
    }

    public void SetResource(Resource resource)
    {
        _resource = resource;
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
                ClearResource();
                return;

            case var value when value == _resourceIndex:
                GoToResource();
                break;

            case var value when value == _baseIndex:
                GoToBase();
                break;
        }
    }

    private void GoToResource()
    {
        transform.position = Vector3.MoveTowards(transform.position, _resource.transform.position, _speed * Time.deltaTime);
        LookAtTarget(_resource.transform);
    }

    private void GoToBase()
    {
        _isResourceReached = true;
        transform.position = Vector3.MoveTowards(transform.position, _baseTransform.position, _speed * Time.deltaTime);
        LookAtTarget(_baseTransform);
    }

    private void LookAtTarget(Transform target)
    {
        transform.LookAt(target.position);
    }
}
