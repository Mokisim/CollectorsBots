using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private Building _basePrefab;
    [SerializeField] private Building _mainBuilding;

    public event Action<Building> BaseBuilded;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit unit) && unit.Target == transform)
        {
            var newBase = Instantiate(_basePrefab, Vector3.zero, transform.rotation);
            newBase.transform.position = transform.position;
            BaseBuilded?.Invoke(_mainBuilding);
        }
    }
}
