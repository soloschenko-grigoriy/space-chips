using UnityEngine;

public class HexCell : MonoBehaviour {
    public static float Width = 1.73f;
    public static float Height = 2f;
    public HexCoordinates coordinates;

    [SerializeField] Color inActiveColor = Color.white;
    [SerializeField] Color activeColor = Color.green;

    public bool IsActive {
        get {
            return isActive;
        }

        set {
            isActive = value;
            rend.material.color = value ? activeColor : inActiveColor;
        }
    }

    bool isActive;
    Renderer rend;

    void Awake() {
        rend = GetComponentInChildren<Renderer>();
    }

    public void ToggleIsActive() {
        IsActive = !IsActive;
    }
}
