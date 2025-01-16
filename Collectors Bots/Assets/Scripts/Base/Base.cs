using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    [SerializeField] private List<Unit> _allUnits = new List<Unit>();

    public event Action ResourceGetted;
    public event Action ResourcesReceived;

    private List<Resource> _aviableRecources = new List<Resource>();
    private List<Unit> _freeUnits = new List<Unit>();

    public List<Unit> AllUnits
    {
        get
        {
            return AllUnits = _allUnits;
        }

        private set { }
    }

    private void Awake()
    {
        foreach (Unit unit in _allUnits)
        {
            _freeUnits.Add(unit);
        }
    }

    private void OnEnable()
    {
        _scanner.ResourcesFound += GetScannedResources;
        ResourcesReceived += SendUnits;

        foreach (Unit unit in _allUnits)
        {
            unit.ArrivedBase += GetUnits;
        }
    }

    private void OnDisable()
    {
        _scanner.ResourcesFound -= GetScannedResources;
        ResourcesReceived -= SendUnits;

        foreach (Unit unit in _allUnits)
        {
            unit.ArrivedBase -= GetUnits;
        }
    }

    private void GetScannedResources(List<Resource> targetRecources)
    {
        _aviableRecources = targetRecources;
        ResourcesReceived.Invoke();
    }

    private void SendUnits()
    {
        if (_freeUnits.Count > 0 && _aviableRecources.Count > 0)
        {
            while (_freeUnits.Count > 0 && _aviableRecources.Count > 0)
            {
                Unit firstUnit = _freeUnits.First();

                firstUnit.Resource = _aviableRecources.First();
                firstUnit.IsResourceReached = false;
                _freeUnits.Remove(firstUnit);
                _aviableRecources.Remove(_aviableRecources.First());
            }
        }
    }

    private void GetUnits(Unit unit)
    {
        _freeUnits.Add(unit);
        ResourceGetted.Invoke();
    }
}
