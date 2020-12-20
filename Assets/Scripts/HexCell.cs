using UnityEngine;

public class HexCell : MonoBehaviour {
    public static float Width = 1.73f;
    public static float Height = 2f;

    [SerializeField] Color inActiveColor = Color.white;
    [SerializeField] Color activeColor = Color.green;

    public int Q {
        get {
            return q;
        }
    }

    public int R {
        get {
            return r;
        }
    }

    public int S {
        get {
            return -q - r;
        }
    }

    public bool IsActive {
        get {
            return isActive;
        }

        set {
            isActive = value;
            rend.material.color = value ? activeColor : inActiveColor;
        }
    }

    int q, r;
    bool isActive;
    Renderer rend;

    void Awake() {
        rend = GetComponentInChildren<Renderer>();
    }

    public void SetCoordinates(int x, int z) {
        q = x;
        r = z;
    }

    public string GetCoordinates() {
        return $"({Q}, {S}, {R})";
    }

    public void ToggleIsActive(){
        IsActive = !IsActive;
    }
}
