using UnityEngine;

public class HexCell : MonoBehaviour {
    public static float Width = 1.73f;
    public static float Height = 2f;
    public HexCoordinates coordinates;
    public HexCell[] Neighbors;

    [SerializeField] Color inactiveColor = Color.white;
    [SerializeField] Color activeColor = Color.green;
    [SerializeField] Color rangeColor = Color.red;
    [SerializeField] Color pathColor = Color.grey;

    public bool IsActive {
        get {
            return isActive;
        }

        set {
            isActive = value;
            rend.material.color = value ? activeColor : inactiveColor;
        }
    }

    public bool IsInRange {
        get {
            return isInRange;
        }

        set {
            isInRange = value;
            rend.material.color = value ? rangeColor : inactiveColor;
        }
    }

    public bool IsInPath {
        get {
            return isInPath;
        }

        set {
            isInPath = value;
            rend.material.color = value ? pathColor : inactiveColor;
        }
    }

    bool isActive, isInRange, isInPath;
    Renderer rend;

    void Awake() {
        rend = GetComponentInChildren<Renderer>();
    }

    public void ToggleIsActive() {
        IsActive = !IsActive;
    }

    public int Cost(HexCell next) {
        return 1;
    }
}
