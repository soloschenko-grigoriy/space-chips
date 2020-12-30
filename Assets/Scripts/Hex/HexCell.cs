using UnityEngine;

public enum HexCellType {
    Target, Range, Path, Default
}

public class HexCell : MonoBehaviour {
    public static float Width = 1.73f;
    public static float Height = 2f;

    public HexCoordinates Coordinates;
    public HexCell[] Neighbors;
    public HexCellType Type = HexCellType.Default;
    public Ship OccupiedBy => _occupiedBy;

    [SerializeField] Color _defaultColor = Color.white;
    [SerializeField] Color _activeColor = Color.green;
    [SerializeField] Color _rangeColor = Color.red;
    [SerializeField] Color _pathColor = Color.grey;
    [SerializeField] Color _occupiedColor = Color.clear;
    [SerializeField] Color _closestNeighbor = Color.magenta;

    Renderer _renderer;
    Ship _occupiedBy;

    void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
    }

    void Update() {
        SetColor();
    }

    public int Cost(HexCell next) {
        return 1;
    }

    public void Occupy(Ship ship) {
        if (_occupiedBy != null) {
            Debug.LogWarning($"This cell {Coordinates} was already occupied");
        }

        _occupiedBy = ship;
    }

    public void Liberate() {
        _occupiedBy = null;
    }

    public HexCell FindClosestNeighbor(HexCell cell) {
        var closest = Neighbors[0];
        int shortestDistance = int.MaxValue;
        for (int i = 0; i < Neighbors.Length; i++) {
            if (Neighbors[i] == null) {
                continue;
            }

            var distance = Neighbors[i].DistanceTo(cell);
            if (shortestDistance > distance) {
                shortestDistance = distance;
                closest = Neighbors[i];
            }
        }

        return closest;
    }

    public int DistanceTo(HexCell other) {
        return HexCoordinates.DistanceBetween(Coordinates, other.Coordinates);
    }

    void SetColor() {
        var color = _renderer.material.color = _defaultColor;
        switch (Type) {
            case HexCellType.Path: color = _pathColor; break;
            case HexCellType.Range: color = _rangeColor; break;
            case HexCellType.Target: color = _activeColor; break;
        }

        if (OccupiedBy != null) {
            color = _occupiedColor;
        }

        if (color != _renderer.material.color) {
            _renderer.material.color = color;
        }
    }
}
