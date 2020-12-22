using UnityEngine;

public class Ships : MonoBehaviour {
    HexCell position;

    void Awake() {
        enabled = false;
    }


    void Spawn(HexCell at) {
        enabled = true;
        transform.position = at.transform.position;
    }
}
