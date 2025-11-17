using System;
using UnityEngine;

public class MouseObjectDetection : MonoBehaviour
{
    private int _leftMouseButton = 0;

    public event Action<Base> BaseClicked;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_leftMouseButton) == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent(out Base clickedBase) == true && clickedBase != null)
                {
                    BaseClicked?.Invoke(clickedBase);
                }
            }
        }
    }
}
