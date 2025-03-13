using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    private List<Resource> _aviableResources = new List<Resource>();
    private List<Resource> _allResources = new List<Resource>();
    private List<Unit> _baseUnits = new List<Unit>();

    public event Action<List<Resource>> ResourcesSorted;

    public void SetUnits(List<Unit> units)
    {
        _baseUnits = units;
    }

    public void SetResources(List<Resource> resources)
    {
        _allResources = resources;
        SortFoundedResources(_allResources);
    }

    private void SortFoundedResources(List<Resource> foundedResources)
    {
        _aviableResources.Clear();

        int recurringResources = 0;

        List<Resource> sortedResources = new List<Resource>();

        foreach (Resource resource in foundedResources)
        {
            foreach (Unit unit in _baseUnits)
            {
                if (unit.Resource == resource)
                {
                    recurringResources++;
                }
            }

            if (recurringResources == 0)
            {
                sortedResources.Add(resource);
            }
        }

        foreach (Resource resource in sortedResources)
        {
            _aviableResources.Add(resource);
        }

        sortedResources.Clear();

        ResourcesSorted.Invoke(_aviableResources);
    }
}
