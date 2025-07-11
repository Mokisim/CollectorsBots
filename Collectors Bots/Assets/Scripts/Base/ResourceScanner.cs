using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private Vector3 _extents;
    [SerializeField] private float _scanDelay = 5f;

    public event Action<Queue<Resource>> ResourcesFound;

    private bool _isActive;
    private WaitForSeconds _scanCooldown;
    private Coroutine _coroutine;
    private int _extentsScaler = 2;

    private void Awake()
    {
        _scanCooldown = new WaitForSeconds(_scanDelay);
    }

    private void Start()
    {
        _coroutine = StartCoroutine(PeriodicScan());
    }

    private void OnEnable()
    {
        _isActive = true;
    }

    private void OnDisable()
    {
        _isActive = false;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void Scan()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, _extents / _extentsScaler, Quaternion.identity);

        Queue<Resource> resources = new Queue<Resource>();

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent(out Resource resource) && collider.gameObject.activeSelf == true)
            {
                resources.Enqueue(resource);
            }
        }

        ResourcesFound.Invoke(resources);
        resources.Clear();
    }

    private IEnumerator PeriodicScan()
    {
        while (_isActive)
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
