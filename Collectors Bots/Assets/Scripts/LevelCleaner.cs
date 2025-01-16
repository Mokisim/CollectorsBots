using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCleaner : MonoBehaviour
{
    [SerializeField] private Vector3 _halfExtents;
    [SerializeField] private ObjectPool _pool;

    public void CleanLevel()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, _halfExtents / 2, Quaternion.identity);

        foreach (Collider collider in hitColliders)
        {
            _pool.PutObject(collider.gameObject.transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(transform.position, _halfExtents);
    }
}
