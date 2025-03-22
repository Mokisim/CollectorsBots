using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsGreed : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(10, 10);
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private List<Building> _buildingsOnLevel;
    [SerializeField] private BuildingsPool _pool;

    private Building[,] _grid;
    private Building _flyingBuilding;

    private bool _available;

    public event Action<Building> BuildingBuilded;

    public bool Available => _available;

    private void Awake()
    {
        _available = true;

        _grid = new Building[_gridSize.x, _gridSize.y];

        foreach (Building building in _buildingsOnLevel)
        {
            Vector3 buildingWorldPosition = building.transform.position;

            int x = Mathf.RoundToInt(buildingWorldPosition.x);
            int y = Mathf.RoundToInt(buildingWorldPosition.z);

            AddBuilding(x, y, building);
        }
    }

    private void Update()
    {
        if (_flyingBuilding != null)
        {
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if ((x < 0 || x > _gridSize.x - _flyingBuilding.Size.x) || (y < 0 || y > _gridSize.y - _flyingBuilding.Size.y) || (available && CheckPlace(x, y)))
                {
                    available = false;
                }

                _available = available;

                _flyingBuilding.transform.position = new Vector3(x, 0, y);
                _flyingBuilding.SetTransparent(available);

                if (available == true && Input.GetMouseButtonDown(1))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (_flyingBuilding != null)
        {
            _pool.PutObject(_flyingBuilding.transform);
        }

        var flyingBuildingTransform = _pool.GetObject(buildingPrefab.transform);
        flyingBuildingTransform.gameObject.SetActive(true);
        _flyingBuilding = flyingBuildingTransform.GetComponent<Building>();
    }

    public void AddBuilding(int placeX, int placeY, Building building)
    {
        for (int x = 0; x < building.Size.x; x++)
        {
            for (int y = 0; y < building.Size.y; y++)
            {
                _grid[placeX + x, placeY + y] = building;
            }
        }
    }

    public void DestroyBuilding(Building building)
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                if (building == _grid[x, y])
                {
                    _pool.PutObject(_grid[x, y].transform);
                    _buildingsOnLevel.Remove(building);
                    _grid[x, y] = null;
                }
            }
        }
    }

    private bool CheckPlace(int placeX, int placeY)
    {
        bool isPlaceTaken;

        for (int x = 0; x < _flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < _flyingBuilding.Size.y; y++)
            {
                if (_grid[placeX + x, placeY + y] != null)
                {
                    isPlaceTaken = true;
                    return isPlaceTaken;
                }
            }
        }

        return isPlaceTaken = false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        AddBuilding(placeX, placeY, _flyingBuilding);
        _buildingsOnLevel.Add(_flyingBuilding);

        _flyingBuilding.SetNormal();
        _flyingBuilding.Build(_flyingBuilding.transform, _flyingBuilding);
        _pool.PutObject(_flyingBuilding.transform);

        BuildingBuilded.Invoke(_flyingBuilding);

        _flyingBuilding = null;
    }
}
