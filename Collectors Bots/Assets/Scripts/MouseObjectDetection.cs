using System;
using UnityEngine;

public class MouseObjectDetection : MonoBehaviour
{
    [SerializeField] private BuildingsGreed _greed;
    [SerializeField] private Building _prefab;
    [SerializeField] private Base _base;

    private int _minBaseFlags = 0;
    private int _maxBaseFlags = 1;

    private void Awake()
    {
        if (_greed == null)
        {
            _greed = FindObjectOfType<BuildingsGreed>();
        }
    }

    private void OnEnable()
    {
        _greed.BuildingBuilded += GiveBuilding;
    }

    private void OnDisable()
    {
        _greed.BuildingBuilded -= GiveBuilding;
    }

    private void OnMouseDown()
    {
        if (_base.BaseFlag == _minBaseFlags && _base.AllUnits.Count > _base.MinUnitsCount)
        {
            _greed.StartPlacingBuilding(_prefab);
        }
        else if (_base.BaseFlag == _maxBaseFlags && _base.AllUnits.Count > _base.MinUnitsCount)
        {
            _greed.DestroyBuilding(_base.GetFlag());
            _base.DeleteFlag();
            _greed.StartPlacingBuilding(_prefab);
        }
    }

    private void GiveBuilding(Building building)
    {
        _base.AddFlag(building);
    }
}
