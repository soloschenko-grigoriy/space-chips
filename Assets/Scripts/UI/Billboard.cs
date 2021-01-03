using UnityEngine;

public class Billboard : MonoBehaviour {
    Camera _cam;

    // Start is called before the first frame update
    void Awake() {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(transform.position + _cam.transform.forward);
    }
}
