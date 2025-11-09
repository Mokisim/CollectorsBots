using System;
using UnityEngine;

public class MouseObjectDetection : MonoBehaviour
{
    [SerializeField] private BuildingsGreed _greed;
    [SerializeField] private Building _prefab;

    private Base _base;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out Base clickedBase) == true)
            {
                if (clickedBase != null) 
                {
                    UpdateBase(clickedBase);
                }

                if (clickedBase.BaseFlag == _minBaseFlags && clickedBase.AllUnits.Count > clickedBase.MinUnitsCount)
                {
                    _greed.StartPlacingBuilding(_prefab);
                }
                else if (clickedBase.BaseFlag == _maxBaseFlags && clickedBase.AllUnits.Count > _base.MinUnitsCount)
                {
                    _greed.DestroyBuilding(_base.GetFlag());
                    clickedBase.DeleteFlag();
                    _greed.StartPlacingBuilding(_prefab);
                }
            }
        }
    }

    private void GiveBuilding(Building building)
    {
        Debug.Log(_base);
        _base.AddFlag(building);
    }

    private void UpdateBase(Base currentBase)
    {
        _base = currentBase;
    }
}
