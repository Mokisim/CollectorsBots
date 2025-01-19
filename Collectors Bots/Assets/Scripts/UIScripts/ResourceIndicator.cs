using TMPro;
using UnityEngine;

public class ResourceIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Base _base;

    private void OnEnable()
    {
        _base.ResourcesUpdated += ShowResources;
    }

    private void OnDisable()
    {
        _base.ResourcesUpdated -= ShowResources;
    }

    private void ShowResources()
    {
        _score.text = $"Ресурсы: {_base.BaseResources}";
    }
}
