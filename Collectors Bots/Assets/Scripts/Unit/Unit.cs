using System;
using UnityEditor.Build;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private TakePoint _takePoint;
    [SerializeField] private Transform _baseTransform;

    private Transform _target;
    
    private Resource _resource;

    public event Action<Transform, Unit> TargetReached;
    
    public TakePoint TakePoint => _takePoint;

    public Resource Resource { get { return _resource; } private set { } }
    
    public Transform Target { get { return _target; } private set { } }

    private void Start()
    {
        if(_baseTransform != null)
        {
            _takePoint.SetUnitBase(_baseTransform);
        }
    }

    private void Update()
    {
        if (_target != null)
        {
            GoTarget();
        }
    }

    private void OnEnable()
    {
        _takePoint.TargetReached += SayTargetReached;
    }

    private void OnDisable()
    {
        _takePoint.TargetReached -= SayTargetReached;
    }

    public void ClearResource()
    {
        _resource = null;
        _takePoint.SetResource(null);
    }

    public void SetResource(Resource resource)
    {
        _resource = resource;
        _takePoint.SetResource(resource);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void ClearTarget()
    {
        _target = null;
    }

    public void SetBaseTransform(Transform transform)
    {
        _baseTransform = transform;
        _takePoint.SetUnitBase(_baseTransform);
    }

    public void DeleteBaseTransform()
    {
        _baseTransform = null;
    }

    private void GoTarget()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
            LookAtTarget(_target.transform);
        }
    }

    private void SayTargetReached(Transform reachedTarget)
    {
        TargetReached.Invoke(reachedTarget, this);
    }

    private void LookAtTarget(Transform target)
    {
        transform.LookAt(target.position);
    }
}
