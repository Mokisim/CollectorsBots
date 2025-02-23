using UnityEngine;

public class Resource : MonoBehaviour
{
    private Transform _takePointTransform;
    private float _zero = 0;

    public void Take(Transform takePoint)
    {
        transform.SetParent(takePoint.parent);
        transform.localPosition.Set(_zero, _zero, _zero);
    }
}
