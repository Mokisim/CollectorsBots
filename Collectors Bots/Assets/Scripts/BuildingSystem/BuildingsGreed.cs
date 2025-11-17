using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsGreed : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(10, 10);
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private List<Building> _buildingsOnLevel;
    [SerializeField] private BuildingsPool _pool;
    [SerializeField] private ObjectPool _resourcesPool;
    [SerializeField] private ResourceWatcher _resourcesWatcher;

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

                if ((x < 0 || x > _gridSize.x - _flyingBuilding.Size.x) || (y < 0 || y > _gridSize.y - _flyingBuilding.Size.y) || (available && IsPlaceAlreadyOccupied(x, y)))
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
            _pool.PutObject(_flyingBuilding);
        }

        var flyingBuildingTransform = _pool.GetObject(buildingPrefab);
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
                    _grid[x, y] = null;
                    _buildingsOnLevel.Remove(building);
                    _pool.PutObject(building);
                }
            }
        }
    }

    public void SwapBuildings(Building originalBuilding, Building newBuilding)
    {
        var buildedBuilding = _pool.GetObject(newBuilding, newBuilding.IsFlag);
        buildedBuilding.gameObject.SetActive(true);
        buildedBuilding.transform.position = originalBuilding.transform.position;

        DestroyBuilding(originalBuilding);

        int newBuildingWorldPositionX = Mathf.RoundToInt(buildedBuilding.transform.position.x);
        int newBuildingWorldPositionY = Mathf.RoundToInt(buildedBuilding.transform.position.z);

        AddBuilding(newBuildingWorldPositionX, newBuildingWorldPositionY, buildedBuilding);
        _buildingsOnLevel.Add(buildedBuilding);
    }

    public void SwapBuildings(Building originalBuilding, Building newBuilding, Unit unit = null)
    {
        var buildedBuilding = _pool.GetObject(newBuilding, newBuilding.IsFlag);
        buildedBuilding.gameObject.SetActive(true);
        buildedBuilding.transform.position = originalBuilding.transform.position;

        DestroyBuilding(originalBuilding);

        int newBuildingWorldPositionX = Mathf.RoundToInt(buildedBuilding.transform.position.x);
        int newBuildingWorldPositionY = Mathf.RoundToInt(buildedBuilding.transform.position.z);

        AddBuilding(newBuildingWorldPositionX, newBuildingWorldPositionY, buildedBuilding);
        _buildingsOnLevel.Add(buildedBuilding);

        if (buildedBuilding.GetComponentInChildren<Base>() != null)
        {
            buildedBuilding.GetComponentInChildren<Base>().SetWatcherAndPool(_resourcesPool, _resourcesWatcher);

            if (unit != null)
            {
                buildedBuilding.GetComponentInChildren<Base>().AddUnit(unit);
            }
        }
    }

    private bool IsPlaceAlreadyOccupied(int placeX, int placeY)
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
        _flyingBuilding.SetNormal();

        var buildedBuilding = _pool.GetObject(_flyingBuilding);
        buildedBuilding.gameObject.SetActive(true);
        buildedBuilding.transform.position = _flyingBuilding.transform.position;

        AddBuilding(placeX, placeY, buildedBuilding);

        _pool.PutObject(_flyingBuilding);
        _buildingsOnLevel.Add(buildedBuilding);

        BuildingBuilded?.Invoke(buildedBuilding);

        _flyingBuilding = null;
    }
}
