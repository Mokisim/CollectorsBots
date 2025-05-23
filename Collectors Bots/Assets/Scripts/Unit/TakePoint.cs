using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TakePoint : MonoBehaviour
{
    private Resource _resource;
    private Resource _unitResource;
    private Transform _unitBase;

    public event Action<Transform> ResourceReturned;
    public event Action<Transform> TargetReached;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource) == true && _unitResource == resource)
        {
            _resource = resource;
            _resource.Take(this.transform);
            TargetReached.Invoke(resource.transform);
        }
        else if (collision.TryGetComponent(out Base @base) == true && _unitBase == @base.transform && _unitResource != null)
        {
            if (_resource != null)
            {
                TargetReached?.Invoke(@base.transform);
                ResourceReturned?.Invoke(_resource.transform);
                _resource = null;
            }
        }
    }

    public void SetResource(Resource resource)
    {
        _unitResource = resource;
    }

    public void SetUnitBase(Transform base1)
    {
        _unitBase = base1;
    }
}
