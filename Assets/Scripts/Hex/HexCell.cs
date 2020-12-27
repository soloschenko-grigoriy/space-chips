using UnityEngine;

public enum HexCellHighlightType {
    Target, Range, Path, Default
}

public class HexCell : MonoBehaviour {
    public static float Width = 1.73f;
    public static float Height = 2f;

    public HexCoordinates coordinates;
    public HexCell[] Neighbors;
    public HexCellHighlightType Type = HexCellHighlightType.Default;

    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color activeColor = Color.green;
    [SerializeField] Color rangeColor = Color.red;
    [SerializeField] Color pathColor = Color.grey;

    Renderer rend;

    void Awake() {
        rend = GetComponentInChildren<Renderer>();
    }

    void Update() {
        SetColor();
    }

    public int Cost(HexCell next) {
        return 1;
    }

    void SetColor() {
        var color = rend.material.color = defaultColor;
        switch (Type) {
            case HexCellHighlightType.Path: color = pathColor; break;
            case HexCellHighlightType.Range: color = rangeColor; break;
            case HexCellHighlightType.Target: color = activeColor; break;
        }

        if (color != rend.material.color) {
            rend.material.color = color;
        }
    }
}
