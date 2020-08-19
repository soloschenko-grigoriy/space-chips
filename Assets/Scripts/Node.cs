using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField, Range(0.5f, 1f)]
    private float _activeAlpha = default;

    [SerializeField, Range(0, 1f)]
    private float _inactiveAlpha = default;

    private bool _isActive;

    private bool IsActive
    {
        get
        {
            return _isActive;
        }

        set
        {
            _isActive = value;
            string colorName = "_Color";
            Color color = _material.GetColor(colorName);
            color.a = value ? _activeAlpha : _inactiveAlpha;
            _material.SetColor(colorName, color);
        }
    }

    private Material _material;

    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        IsActive = false;
    }

    public void ToggleActive()
    {
        IsActive = !IsActive;
    }
}
