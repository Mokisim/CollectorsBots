using UnityEngine;

public class Resource : MonoBehaviour
{
    public void Take(Transform takePoint)
    {
        transform.SetParent(takePoint.parent);
        transform.localPosition = Vector3.zero;
    }
}
