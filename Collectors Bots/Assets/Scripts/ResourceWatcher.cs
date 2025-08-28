using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceWatcher : MonoBehaviour
{
    [SerializeField] private ResourceScanner _scanner;

    public event Action NewResourcesFounded;

    private List<Resource> _aviableResources = new List<Resource>();
    private List<Resource> _resourcesOnlevel = new List<Resource>();
    private List<Resource> _givedResources = new List<Resource>();

    private int _resourcesCount;

    public int ResourcesCount { get { return _resourcesCount; } private set { } }

    private void Awake()
    {

    }

    private void OnEnable()
    {
        _scanner.ResourcesFound += UpdateResourcesList;
    }

    private void OnDisable()
    {
        _scanner.ResourcesFound += UpdateResourcesList;
    }

    public Resource GetAviableResource()
    {
        Resource givedResource = _aviableResources.First();

        if (_resourcesCount > 0)
        {
            _givedResources.Add(givedResource);
            _aviableResources.Remove(givedResource);
            return givedResource;

        }
        else
        {
            return null;
        }
    }

    public int CheckResource()
    {
        return _aviableResources.Count();
    }

    private void RemoveDuplicateResources()
    {
        List<Resource> aviableResources = new List<Resource>();
        aviableResources = _resourcesOnlevel;

        if (aviableResources.Count > 0 && _givedResources.Count > 0)
        {
            foreach (Resource resource in aviableResources)
            {
                foreach (Resource resource1 in _givedResources)
                {
                    if (resource == resource1)
                    {
                        aviableResources.Remove(resource);
                    }
                }
            }
        }

        _aviableResources = aviableResources;
        aviableResources.Clear();
    }

    private void UpdateResourcesList(List<Resource> resources)
    {
        _resourcesOnlevel = resources;
        RemoveDuplicateResources();
        UpdateResourcesCount();
        NewResourcesFounded.Invoke();
    }

    private void UpdateResourcesCount()
    {
        _resourcesCount = _resourcesOnlevel.Count;
    }
}
