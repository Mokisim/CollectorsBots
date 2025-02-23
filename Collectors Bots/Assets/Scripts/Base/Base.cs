using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceScanner _scanner;
    [SerializeField] private List<Unit> _allUnits = new List<Unit>();

    private List<Unit> _allUnitsCopy = new List<Unit>();
    private List<Resource> _aviableRecources = new List<Resource>();
    private List<Unit> _freeUnits = new List<Unit>();
    private int _baseResources;

    public event Action ResourcesUpdated;
    public List<Unit> AllUnits => _allUnitsCopy;
    public int BaseResources => _baseResources;

    private void Awake()
    {
        _allUnitsCopy = _allUnits;
        _baseResources = 0;

        foreach (Unit unit in _allUnits)
        {
            _freeUnits.Add(unit);
        }
    }

    private void OnEnable()
    {
        _scanner.ResourcesFound += GetScannedResources;
        
        foreach (Unit unit in _allUnits)
        {
            unit.ArrivedBase += GetUnits;
            unit.GiveResource += AddResource;
        }
    }

    private void OnDisable()
    {
        _scanner.ResourcesFound -= GetScannedResources;
        
        foreach (Unit unit in _allUnits)
        {
            unit.ArrivedBase -= GetUnits;
            unit.GiveResource -= AddResource;
        }
    }

    private void GetScannedResources(List<Resource> targetRecources)
    {
        _aviableRecources = targetRecources;
        SendUnits();
    }

    private void SendUnits()
    {
        if (_freeUnits.Count > 0 && _aviableRecources.Count > 0)
        {
            while (_freeUnits.Count > 0 && _aviableRecources.Count > 0)
            {
                Unit firstUnit = _freeUnits.First();

                firstUnit.SetResource(_aviableRecources.First());
                firstUnit.SetUnreached();
                _freeUnits.Remove(firstUnit);
                _aviableRecources.Remove(_aviableRecources.First());
            }
        }
    }

    private void GetUnits(Unit unit)
    {
        _freeUnits.Add(unit);
    }

    private void AddResource()
    {
        _baseResources++;
        ResourcesUpdated.Invoke();
    }
}
