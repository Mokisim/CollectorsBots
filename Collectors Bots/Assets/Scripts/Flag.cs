using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private Building _basePrefab;
    [SerializeField] private Building _mainBuilding;

    public event Action<Building, Building> BaseBuilded;
    public event Action FlagDestroyed;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Unit unit) && unit.Target == transform)
        {
            unit.ClearTarget();
            var newBase = Instantiate(_basePrefab, Vector3.zero, transform.rotation);
            newBase.transform.position = transform.position;
            newBase.AddNewBaseUnit(unit);

            FlagDestroyed?.Invoke();
            BaseBuilded?.Invoke(_mainBuilding, newBase);
        }
    }
}
