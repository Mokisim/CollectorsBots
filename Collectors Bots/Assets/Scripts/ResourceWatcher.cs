using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceWatcher : MonoBehaviour
{
    [SerializeField] private ResourceScanner _scanner;

    public event Action NewResourcesFounded;

    private Queue<Resource> _resourcesOnlevel;

    private int _resourcesCount;

    public int ResourcesCount { get { return _resourcesCount; } private set { } }

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
        return _resourcesOnlevel.Dequeue();
    }

    public Resource CheckResource()
    {
        return _resourcesOnlevel.Peek();
    }

    private void UpdateResourcesList(Queue<Resource> resources)
    {
        _resourcesOnlevel = resources;
        UpdateResourcesCount();
        NewResourcesFounded.Invoke();
    }

    private void UpdateResourcesCount()
    {
        _resourcesCount = _resourcesOnlevel.Count;
    }
}
