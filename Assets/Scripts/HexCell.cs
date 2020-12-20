using UnityEngine;

public class HexCell : MonoBehaviour {
    public static float Width = 1.73f;
    public static float Height = 2f;
    public HexCoordinates coordinates;

    [SerializeField] Color inActiveColor = Color.white;
    [SerializeField] Color activeColor = Color.green;
    [SerializeField] Color rangeColor = Color.red;

    public bool IsActive {
        get {
            return isActive;
        }

        set {
            isActive = value;
            rend.material.color = value ? activeColor : inActiveColor;
        }
    }

    public bool IsInRange {
        get {
            return isInRange;
        }

        set {
            isInRange = value;
            rend.material.color = value ? rangeColor : inActiveColor;
        }
    }

    bool isActive, isInRange;
    Renderer rend;

    void Awake() {
        rend = GetComponentInChildren<Renderer>();
    }

    public void ToggleIsActive() {
        IsActive = !IsActive;
    }
}
