using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingsPool : MonoBehaviour
{
    [SerializeField] private Transform _container;

    private List<Building> _pool;

    public IEnumerable<Component> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new List<Building>();
    }

    public Building GetObject(Building prefab)
    {
        if (_pool.Count == 0)
        {
            var component = Instantiate(prefab, Vector3.zero, transform.rotation);
            component.transform.parent = _container;

            return component;
        }

        Building lastBuilding = _pool.Last();
        _pool.Remove(lastBuilding);

        return lastBuilding;
    }

    public Building GetObject(Building prefab, bool isFlag)
    {
        if (_pool.Count == 0)
        {
            var component = Instantiate(prefab, Vector3.zero, transform.rotation);
            component.transform.parent = _container;

            return component;
        }

        foreach (Building building in _pool)
        {
            if (building.IsFlag == isFlag)
            {
                Building neededBuilding = building;
                _pool.Remove(neededBuilding);
                return neededBuilding;
            }
        }

        var component1 = Instantiate(prefab, Vector3.zero, transform.rotation);
        component1.transform.parent = _container;

        return component1;
    }

    public void PutObject(Building component)
    {
        _pool.Add(component);
        component.gameObject.SetActive(false);
    }
}
