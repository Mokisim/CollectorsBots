using System.Collections.Generic;
using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private MouseObjectDetection _mouseObjectDetection;
    [SerializeField] private BuildingsGreed _buldingsGreed;
    [SerializeField] private Building _flagPrefab;
    [SerializeField] private Building _basePrefab;

    private Base _base;
    private List<Base> _baseList = new List<Base>();

    private void OnEnable()
    {
        _mouseObjectDetection.BaseClicked += StartPlacingFlag;
        _buldingsGreed.BuildingBuilded += UpdateBaseFlag;
    }

    private void OnDisable()
    {
        _mouseObjectDetection.BaseClicked -= StartPlacingFlag;
        _buldingsGreed.BuildingBuilded -= UpdateBaseFlag;
    }

    private void StartPlacingFlag(Base currentBase)
    {
        _base = currentBase;

        if (currentBase.Flag == null && currentBase.AllUnits.Count > currentBase.MinUnitsCount)
        {
            _buldingsGreed.StartPlacingBuilding(_flagPrefab);
        }
        else if (currentBase.Flag != null && currentBase.AllUnits.Count > currentBase.MinUnitsCount)
        {
            _buldingsGreed.DestroyBuilding(_base.GetFlag());
            currentBase.DeleteFlag();
            _buldingsGreed.StartPlacingBuilding(_flagPrefab);
        }
    }

    private void UpdateBaseFlag(Building flag)
    {
        if (flag.IsFlag == true)
        {
            _base.AddFlag(flag);
            _baseList.Add(_base);
            _base = null;
            flag.UnitArrived += ReplaceFlag;
        }
    }

    private void ReplaceFlag(Building flag, Unit unit)
    {
        _buldingsGreed.SwapBuildings(flag, _basePrefab, unit);
        flag.UnitArrived -= ReplaceFlag;

        foreach (Base base1 in _baseList)
        {
            if (base1.GetFlag() == flag)
            {
                base1.DeleteFlag();
                _baseList.Remove(base1);
                return;
            }
        }
    }
}
