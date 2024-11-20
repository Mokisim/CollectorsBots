using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    private event Action ResourcesReceived;

    [SerializeField] private BaseScanner _scanner;
    [SerializeField] private List<Unit> _allUnits = new List<Unit>();

    private List<Resource> _aviableRecources = new List<Resource>();
    private List<Unit> _freeUnits = new List<Unit>();

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

        foreach(Unit unit in _allUnits)
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
        if (_freeUnits.Count > 0)
        {
            while(_freeUnits.Count > 0)
            {
                _freeUnits.First().Resource = _aviableRecources.First();
                _freeUnits.First().IsResourceReached = false;
                _freeUnits.Remove(_freeUnits.First());
                _aviableRecources.Remove(_aviableRecources.First());
            }
        }
    }

    private void GetUnits(Unit unit)
    {
        _freeUnits.Add(unit);
    }
}
