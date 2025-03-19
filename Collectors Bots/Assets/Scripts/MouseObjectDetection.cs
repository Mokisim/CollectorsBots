using UnityEngine;

public class MouseObjectDetection : MonoBehaviour
{
    [SerializeField] private BuildingsGreed _greed;
    [SerializeField] private Building _prefab;

    private void Awake()
    {
        if (_greed == null)
        {
            _greed = FindObjectOfType<BuildingsGreed>();
        }
    }

    private void OnMouseDown()
    {
        if (_greed.Available == true)
        {
            Debug.Log("base click");
            _greed.StartPlacingBuilding(_prefab);
        }
    }
}
