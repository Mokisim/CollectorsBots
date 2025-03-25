using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TakePoint : MonoBehaviour
{
    private Resource _resource;
    private Resource _unitResource;
    private bool _unitIsGoingBuild;
    private Transform _unitTarget;
    
    public event Action<Transform> ResourceReturned;
    public event Action<Transform> TargetReached;
    public event Action FlagReached;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource) == true && _unitResource == resource)
        {
            _resource = resource;
            _resource.Take(this.transform);
            TargetReached.Invoke(resource.transform);
        }
        else if (collision.TryGetComponent(out Base @base) == true && _unitResource != null)
        {
            if (_resource != null)
            {
                TargetReached?.Invoke(@base.transform);
                ResourceReturned?.Invoke(_resource.transform);
                _resource = null;
            }
        }
        else if(collision.TryGetComponent(out Flag flag) == true && _unitIsGoingBuild == true && flag.transform == _unitTarget)
        {
            FlagReached?.Invoke();
        }
    }

    public void SetResource(Resource resource)
    {
        _unitResource = resource;
    }

    public void SetBuildInfo(bool isGoingBuild)
    {
        _unitIsGoingBuild = isGoingBuild;
    }

    public void SetUnitTarget(Transform target)
    {
        _unitTarget = target;
    }
}
