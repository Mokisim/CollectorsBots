using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private ResourceWatcher _watcher;
    [SerializeField] private List<Unit> _allUnits = new List<Unit>();
    [SerializeField] private UnitCreator _unitCreator;

    private List<Unit> _freeUnits = new List<Unit>();
    private int _newUnitPrice = 3;
    private int _newBasePrice = 5;
    private int _baseResources;
    private int _baseFlag = 0;
    private int _minUnitsCount = 1;
    private Building _flag;
    private Flag _mainFlag;

    public event Action ResourcesUpdated;

    public IReadOnlyList<Unit> AllUnits => _allUnits.AsReadOnly();
    public int BaseResources => _baseResources;
    public int BaseFlag => _baseFlag;
    public int MinUnitsCount => _minUnitsCount;

    private void Awake()
    {
        if (_watcher == null)
        {
            _watcher = FindObjectOfType<ResourceWatcher>();
        }

        _baseResources = 0;

        foreach (Unit unit in _allUnits)
        {
            _freeUnits.Add(unit);
        }

        if (_pool == null)
        {
            _pool = FindObjectOfType<ObjectPool>();
        }
    }

    private void Update()
    {
        if (_flag == null)
        {
            AttemptUnitCreation();
        }
    }

    private void OnEnable()
    {
        _watcher.NewResourcesFounded += SendUnits;
        _unitCreator.UnitCreated += AddUnit;

        foreach (Unit unit in _allUnits)
        {
            unit.TargetReached += ValidateUnitTarget;
        }
    }

    private void OnDisable()
    {
        _watcher.NewResourcesFounded -= SendUnits;
        _unitCreator.UnitCreated -= AddUnit;

        if (_mainFlag != null)
        {
            _mainFlag.FlagDestroyed -= DeleteFlag;
        }

        foreach (Unit unit in _allUnits)
        {
            unit.TargetReached -= ValidateUnitTarget;
        }
    }

    public void AddFlag(Building flag)
    {
        _flag = flag;
        _baseFlag++;

        if (_flag.TryGetComponent(out Flag mainFlag) == true)
        {
            _mainFlag = mainFlag;
            _mainFlag.FlagDestroyed += DeleteFlag;
        }
    }

    public void DeleteFlag()
    {
        _flag = null;
        _mainFlag = null;
        _baseFlag--;
    }

    public Building GetFlag()
    {
        return _flag;
    }

    public void AddUnit(Unit unit)
    {
        int recurringUnits = 0;

        if (_allUnits.Count > 0)
        {
            foreach (Unit checkingUnit in _allUnits)
            {
                if (checkingUnit == unit)
                {
                    recurringUnits++;
                }
            }
        }

        if (recurringUnits == 0)
        {
            unit.SetBaseTransform(this.transform);
            unit.ClearResource();
            unit.TargetReached += ValidateUnitTarget;
            _allUnits.Add(unit);
            _freeUnits.Add(unit);
            _pool.AddUnit(unit);
        }
        else
        {
            _freeUnits.Add(unit);
            unit.ClearResource();
        }
    }

    private void SendUnits()
    {
        while (_freeUnits.Count > 0 && _watcher.ResourcesCount > 0)
        {
            Unit firstUnit = _freeUnits.First();

            Resource unitResource = _watcher.GetAviableResource();

            if (unitResource != null)
            {
                firstUnit.SetResource(unitResource);
                firstUnit.SetTarget(unitResource.transform);
                _freeUnits.Remove(firstUnit);
                unitResource = null;
            }
        }
    }

    private void SendFreeUnitBuildNewBase()
    {
        if (_allUnits.Count > _minUnitsCount)
        {
            Unit freeUnit = _freeUnits.First();

            freeUnit.SetTarget(_flag.transform);
            freeUnit.DeleteBaseTransform();
            _freeUnits.Remove(freeUnit);
            _allUnits.Remove(freeUnit);

            _baseResources -= _newBasePrice;
            ResourcesUpdated.Invoke();
        }
    }

    private void ValidateUnitTarget(Transform targetTransform, Unit unit)
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

    private void AddResource()
    {
        _baseResources++;
        ResourcesUpdated.Invoke();

        if (_baseResources >= _newBasePrice && _flag != null)
        {
            SendFreeUnitBuildNewBase();
        }
    }

    private void AttemptUnitCreation()
    {
        if (_baseResources >= _newUnitPrice)
        {
            _baseResources -= _newUnitPrice;
            _unitCreator.CreateUnit(this.transform);
            ResourcesUpdated.Invoke();
        }
    }
}
