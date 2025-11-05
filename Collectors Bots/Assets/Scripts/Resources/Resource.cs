using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _active = true;

    public bool Active { get { return _active; } private set { } }

    private void OnEnable()
    {
        _active = true;
        Active = _active;
    }

    private void OnDisable()
    {
        _active = false;
        Active = _active;
    }

    public void Take(Transform takePoint)
    {
        transform.SetParent(takePoint.parent);
        transform.localPosition = Vector3.zero;
    }
}
