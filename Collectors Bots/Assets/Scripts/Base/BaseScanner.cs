using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BaseScanner : MonoBehaviour
{
    public event Action<List<Resource>> ResourcesFound;

    [SerializeField] private Button _scanButton;
    [SerializeField] private Vector3 _halfExtents;

    private List<Resource> _resourcesFound;

    private void Awake()
    {
        _resourcesFound = new List<Resource>();
    }

    private void OnEnable()
    {
        _scanButton?.onClick.AddListener(Scan);
    }

    private void OnDisable()
    {
        _scanButton?.onClick.RemoveListener(Scan);
    }

    private void Scan()
    {
        _resourcesFound.Clear();

        Collider[] hitColliders = Physics.OverlapBox(transform.position, _halfExtents / 2, Quaternion.identity);

        List<Resource> resources = new List<Resource>();

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent(out Resource resource) && collider.gameObject.activeSelf == true)
            {
                resources.Add(collider.GetComponent<Resource>());
            } 
        }

        foreach (Resource resource in resources)
        {
            _resourcesFound.Add(resource);
        }

        ResourcesFound.Invoke(_resourcesFound);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, _halfExtents);
    }
}
