using UnityEngine;

public enum HexCellType {
    Target, Range, Path, OccupiedByAlly, OccupiedByEnemy, Default
}

public class HexCell : MonoBehaviour {
    public static float Width = 1.73f;
    public static float Height = 2f;

    public HexCoordinates Coordinates;
    public HexCell[] Neighbors;
    public HexCellType Type = HexCellType.Default;

    [SerializeField] Color _defaultColor = Color.white;
    [SerializeField] Color _activeColor = Color.green;
    [SerializeField] Color _rangeColor = Color.red;
    [SerializeField] Color _pathColor = Color.grey;
    [SerializeField] Color _occupiedColor = Color.clear;

    Renderer _renderer;

    void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
    }

    void Update() {
        SetColor();
    }

    public int Cost(HexCell next) {
        return 1;
    }

    void SetColor() {
        var color = _renderer.material.color = _defaultColor;
        switch (Type) {
            case HexCellType.Path: color = _pathColor; break;
            case HexCellType.Range: color = _rangeColor; break;
            case HexCellType.Target: color = _activeColor; break;
            case HexCellType.OccupiedByAlly: color = _occupiedColor; break;
            case HexCellType.OccupiedByEnemy: color = _occupiedColor; break;
        }

        if (color != _renderer.material.color) {
            _renderer.material.color = color;
        }
    }
}
