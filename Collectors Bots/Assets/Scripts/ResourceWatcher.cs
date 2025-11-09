using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceWatcher : MonoBehaviour
{
    [SerializeField] private ResourceScanner _scanner;
    [SerializeField] private ResourceSpawner _spawner;

    public event Action NewResourcesFounded;

    private List<Resource> _aviableResources = new List<Resource>();
    private List<Resource> _resourcesOnlevel = new List<Resource>();
    private List<Resource> _givedResources = new List<Resource>();

    private int _resourcesCount;

    public int ResourcesCount { get { return _resourcesCount; } private set { } }

    private void OnEnable()
    {
        _scanner.ResourcesFound += UpdateResourcesList;
        _spawner.Spawned += UpdateGivedResources;
    }

    private void OnDisable()
    {
        _scanner.ResourcesFound -= UpdateResourcesList;
        _spawner.Spawned -= UpdateGivedResources;
    }

    public Resource GetAviableResource()
    {
        UpdateGivedResources();
        UpdateResourcesCount();

        if (_resourcesCount > 0)
        {
            Resource givedResource = _aviableResources.First();
            _givedResources.Add(givedResource);
            _aviableResources.Remove(givedResource);
            return givedResource;
        }
        else
        {
            return null;
        }
    }

    public int GetAviableResourcesCount()
    {
        return _aviableResources.Count();
    }

    private void RemoveDuplicateResources()
    {
        List<Resource> aviableResources = new List<Resource>();
        List<Resource> aviableResources1 = new List<Resource>();

        foreach (Resource resource in _resourcesOnlevel)
        {
            aviableResources.Add(resource);
            aviableResources1.Add(resource);
        }

        if (aviableResources.Count > 0 && _givedResources.Count > 0)
        {
            foreach (Resource resource in aviableResources1)
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

        _aviableResources.Clear();

        foreach (Resource resource in aviableResources)
        {
            _aviableResources.Add(resource);
        }

        aviableResources.Clear();
    }

    private void UpdateResourcesList(List<Resource> resources)
    {
        _resourcesOnlevel = resources;

        UpdateGivedResources();
        RemoveDuplicateResources();
        UpdateResourcesCount();
        NewResourcesFounded.Invoke();
    }

    private void UpdateResourcesCount()
    {
        _resourcesCount = _aviableResources.Count;
    }

    private void UpdateGivedResources()
    {
        List<Resource> _givedResources1 = new List<Resource>();

        foreach (Resource resource in _givedResources)
        {
            if (resource.gameObject?.activeSelf == true)
            {
                _givedResources1.Add(resource);
            }
        }

        _givedResources.Clear();

        foreach (Resource resource in _givedResources1)
        {
            _givedResources.Add(resource);
        }
    }
}
