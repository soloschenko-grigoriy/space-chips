using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour {
    public static float Width = 1.73f;
    public static float Height = 2f;

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

    int q, r;

    public void SetCoordinates(int x, int z) {
        q = x;
        z = r;
    }
}
