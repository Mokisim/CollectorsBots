using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = Vector2Int.one;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private bool _isFlag = false;

    private Color _normalColor;

    public event Action<Building, Unit> UnitArrived;

    public Vector2Int Size => _size;
    public bool IsFlag => _isFlag;

    private void Awake()
    {
        _normalColor = _renderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isFlag == true)
        {
            if (collision.transform.TryGetComponent(out Unit unit) && unit.Target == transform)
            {
                UnitArrived?.Invoke(this, unit);
            }
        }
    }

    public void SetTransparent(bool available)
    {
        if (available)
        {
            _renderer.material.color = Color.green;
        }
        else
        {
            _renderer.material.color = Color.red;
        }
    }

    public void SetNormal()
    {
        _renderer.material.color = _normalColor;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
            }
        }
    }
}
