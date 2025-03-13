using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceScanner _scanner;
    [SerializeField] private List<Unit> _allUnits = new List<Unit>();
    [SerializeField]private ResourceStorage _storage;

    private List<Resource> _aviableRecources = new List<Resource>();
    private List<Unit> _freeUnits = new List<Unit>();
    private int _baseResources;

    public event Action ResourcesUpdated;
    public IReadOnlyList<Unit> AllUnits => _allUnits.AsReadOnly();
    public int BaseResources => _baseResources;

    private void Awake()
    {
        _baseResources = 0;

        foreach (Unit unit in _allUnits)
        {
            _freeUnits.Add(unit);
        }

        _storage.SetUnits(_allUnits);
    }

    private void OnEnable()
    {
        _scanner.ResourcesFound += _storage.SetResources;
        _storage.ResourcesSorted += SetSortedResources;

        foreach (Unit unit in _allUnits)
        {
            unit.TargetReached += CheckUnitTarget;
        }
    }

    private void OnDisable()
    {
        _scanner.ResourcesFound -= _storage.SetResources;
        _storage.ResourcesSorted -= SetSortedResources;

        foreach (Unit unit in _allUnits)
        {
            unit.TargetReached -= CheckUnitTarget;
        }
    }

    private void SetSortedResources(List<Resource> targetRecources)
    {
        _aviableRecources = targetRecources;
        SendUnits();
    }

    private void SendUnits()
    {
        while (_freeUnits.Count > 0 && _aviableRecources.Count > 0)
        {
            Unit firstUnit = _freeUnits.First();

            if (_aviableRecources.First().gameObject.activeSelf == true)
            {
                firstUnit.SetResource(_aviableRecources.First());
                firstUnit.SetTarget(_aviableRecources.First().transform);
                _freeUnits.Remove(firstUnit);
                _aviableRecources.Remove(_aviableRecources.First());
            }
            else
            {
                _aviableRecources.Remove(_aviableRecources.First());
            }
        }
    }

    private void CheckUnitTarget(Transform targetTransform, Unit unit)
    {
        if (targetTransform == transform)
        {
            unit.SetTarget(null);
            AddFreeUnit(unit);
            AddResource();
        }
        else
        {
            unit.SetTarget(transform);
        }
    }

    private void AddFreeUnit(Unit unit)
    {
        _freeUnits.Add(unit);
        unit.ClearResource();
    }

    private void AddResource()
    {
        _baseResources++;
        ResourcesUpdated.Invoke();
    }
}
