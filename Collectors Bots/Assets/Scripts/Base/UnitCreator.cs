using System;
using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    [SerializeField] private Transform _unitPrefab;

    public event Action<Unit> UnitCreated;

    public void CreateUnit(Transform spawnPoint)
    {
        var unit = Instantiate(_unitPrefab, spawnPoint.position, Quaternion.identity);

        UnitCreated.Invoke(unit.GetComponent<Unit>());
    }
}
