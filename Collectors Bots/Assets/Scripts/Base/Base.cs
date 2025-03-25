using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private ResourceScanner _scanner;
    [SerializeField] private List<Unit> _allUnits = new List<Unit>();
    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private UnitCreator _unitCreator;

    private List<Unit> _freeUnits = new List<Unit>();
    private int _newUnitPrice = 3;
    private int _newBasePrice = 5;
    private int _baseResources;
    private int _baseFlag = 0;
    private Building _flag;

    public event Action ResourcesUpdated;

    public IReadOnlyList<Unit> AllUnits => _allUnits.AsReadOnly();
    public int BaseResources => _baseResources;
    public int BaseFlag => _baseFlag;

    private void Awake()
    {
        _baseResources = 0;

        foreach (Unit unit in _allUnits)
        {
            _freeUnits.Add(unit);
        }

        _storage.SetUnits(_allUnits);

        if (_pool == null)
        {
            _pool = FindObjectOfType<ObjectPool>();
        }
    }

    private void Update()
    {
        if (_flag == null)
        {
            CheckPossibilityCreatingUnit();
        }
        else
        {
            if(_baseResources >= _newBasePrice)
            {

            }
        }

    }

    private void OnEnable()
    {
        _scanner.ResourcesFound += _storage.SetResources;
        _storage.ResourcesSorted += SendUnits;
        _unitCreator.UnitCreated += AddUnit;

        foreach (Unit unit in _allUnits)
        {
            unit.TargetReached += CheckUnitTarget;
        }
    }

    private void OnDisable()
    {
        _scanner.ResourcesFound -= _storage.SetResources;
        _storage.ResourcesSorted -= SendUnits;

        foreach (Unit unit in _allUnits)
        {
            unit.TargetReached -= CheckUnitTarget;
        }
    }

    public void AddFlag(Building flag)
    {
        _flag = flag;
        _baseFlag++;
    }

    public void DeleteFlag()
    {
        _flag = null;
        _baseFlag--;
    }

    public Building GetFlag()
    {
        return _flag;
    }

    private void SendUnits()
    {
        while (_freeUnits.Count > 0 && _storage.AviableResources.Count > 0)
        {
            Unit firstUnit = _freeUnits.First();

            if (_storage.AviableResources.First().gameObject.activeSelf == true)
            {
                firstUnit.SetResource(_storage.AviableResources.First());
                firstUnit.SetTarget(_storage.AviableResources.First().transform);
                _freeUnits.Remove(firstUnit);
                _storage.DeleteAssignedResource(_storage.AviableResources.First());
            }
            else
            {
                _storage.DeleteAssignedResource(_storage.AviableResources.First());
            }
        }
    }

    private void SendFreeUnitBuildNewBase()
    {
        Unit freeUnit = _freeUnits.First();

        freeUnit.SetTarget(_flag.transform);
        freeUnit.DeleteBaseTransform();
        _freeUnits.Remove(freeUnit);
        _allUnits.Remove(freeUnit);
    }

    private void CheckUnitTarget(Transform targetTransform, Unit unit)
    {
        if (targetTransform == transform)
        {
            unit.SetTarget(null);
            AddUnit(unit);
            AddResource();
        }
        else
        {
            unit.SetTarget(transform);
        }
    }

    private void AddUnit(Unit unit)
    {
        int recurringUnits = 0;

        foreach (Unit checkingUnit in _allUnits)
        {
            if (checkingUnit == unit)
            {
                recurringUnits++;
            }
        }

        if (recurringUnits == 0)
        {
            unit.SetBaseTransform(this.transform);
            unit.ClearResource();
            unit.TargetReached += CheckUnitTarget;
            _allUnits.Add(unit);
            _freeUnits.Add(unit);
            _pool.AddUnit(unit);
            _storage.SetUnits(_allUnits);
        }
        else
        {
            _freeUnits.Add(unit);
            unit.ClearResource();
        }
    }

    private void AddResource()
    {
        _baseResources++;
        ResourcesUpdated.Invoke();
    }

    private void CheckPossibilityCreatingUnit()
    {
        if (_baseResources >= _newUnitPrice)
        {
            _baseResources -= _newUnitPrice;
            _unitCreator.CreateUnit(this.transform);
            ResourcesUpdated.Invoke();
        }
    }
}
