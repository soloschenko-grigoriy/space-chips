using UnityEngine;

public class KeyboardInputManager : InputManager {
    public static event MoveInputHandler OnMoveInput;
    public static event RotateInputHandler OnRotateInput;
    public static event ZoomInputHandler OnZoomInput;

    void Update() {
        Move();
        Rotate();
        Zoom();
    }

    void Move() {
        if (Input.GetKey(KeyCode.W)) {
            OnMoveInput?.Invoke(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S)) {
            OnMoveInput?.Invoke(-Vector3.forward);
        }
        if (Input.GetKey(KeyCode.A)) {
            OnMoveInput?.Invoke(-Vector3.right);
        }
        if (Input.GetKey(KeyCode.D)) {
            OnMoveInput?.Invoke(Vector3.right);
        }
    }

    void Rotate() {
        if (Input.GetKey(KeyCode.E)) {
            OnRotateInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.Q)) {
            OnRotateInput?.Invoke(1f);
        }
    }

    void Zoom() {
        if (Input.GetKey(KeyCode.Z)) {
            OnZoomInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.X)) {
            OnZoomInput?.Invoke(1f);
        }
    }
}
