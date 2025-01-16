using TMPro;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Base _base;

    private int _startResources;

    private void Awake()
    {
        _startResources = 0;
    }

    private void OnEnable()
    {
        _base.ResourceGetted += AddResource;
    }

    private void OnDisable()
    {
        _base.ResourceGetted -= AddResource;
    }

    private void AddResource()
    {
        _score.text = $"Ресурсы: {++_startResources}";
    }
}
