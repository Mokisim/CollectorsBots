using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private Vector3 _extents;
    [SerializeField] private Base _base;
    [SerializeField] private float _scanRate = 5f;

    public event Action<List<Resource>> ResourcesFound;

    private List<Unit> _unitsOnLevel = new List<Unit>();
    private bool _isActive;
    private WaitForSeconds _scanCooldown;
    private Coroutine _coroutine;
    private int _extentsScaler = 2;
    
    private void Awake()
    {
        _scanCooldown = new WaitForSeconds(_scanRate);
    }

    private void Start()
    {
        _coroutine = StartCoroutine(PeriodicScan());
        _unitsOnLevel = _base.AllUnits;
    }

    private void OnEnable()
    {
        _isActive = true;
    }

    private void OnDisable()
    {
        _isActive = false;
        StopCoroutine(_coroutine);
    }

    private void Scan()
    {
        int recurringResources = 0;
        Collider[] hitColliders = Physics.OverlapBox(transform.position, _extents / _extentsScaler, Quaternion.identity);

        List<Resource> resources = new List<Resource>();

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent(out Resource resource) && collider.gameObject.activeSelf == true)
            {
                foreach(Unit unitOnLevel in _unitsOnLevel)
                {
                    if(unitOnLevel.Resource == resource)
                    {
                        recurringResources++;
                    }
                }

                if (recurringResources == 0)
                {
                    resources.Add(resource);
                }
            }
        }

        ResourcesFound.Invoke(resources);
        resources.Clear();
    }

    private IEnumerator PeriodicScan()
    {
        while (_isActive == true)
        {
            Scan();

            yield return _scanCooldown;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, _extents);
    }
}
