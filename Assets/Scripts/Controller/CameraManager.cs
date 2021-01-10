using System;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [Header("Camera Positioning")]
    [SerializeField] Vector3 _cameraOffset = new Vector3(0f, 14f, -14f);
    [SerializeField] float _lookAtOffset = 2f;

    [Header("Move Controls")]
    [SerializeField] float _inOutSpeed = 5f;
    [SerializeField] float _lateralSpeed = 5f;
    [SerializeField] float _rotateSpeed = 5f;

    [Header("Move Bounds")]
    [SerializeField] Vector2 _minBounds = default, _maxBounds = default;

    [Header("Zoom Controls")]
    [SerializeField] float _zoomSpeed = 4f;
    [SerializeField] float _nearZoomLimit = 2f;
    [SerializeField] float _farZoomLimit = 16f;
    [SerializeField] float _startingZoom = 5f;

    IZoomStrategy _zoomStrategy;
    Vector3 _frameMove;
    float _frameRotate;
    float _frameZoom;
    Camera _camera;

    void Awake() {
        _camera = GetComponentInChildren<Camera>();
        _camera.transform.localPosition = new Vector3(_cameraOffset.x, _cameraOffset.y, _cameraOffset.z);
        _zoomStrategy = new OrthographicStrategy(_camera, _startingZoom);
        _camera.transform.LookAt(transform.position + Vector3.up * _lookAtOffset);
    }

    void OnEnable() {
        KeyboardInputManager.OnMoveInput += UpdateFrameMove;
        KeyboardInputManager.OnRotateInput += UpdateFrameRotate;
        KeyboardInputManager.OnZoomInput += UpdateFrameZoom;
    }

    void OnDisable() {
        KeyboardInputManager.OnMoveInput -= UpdateFrameMove;
        KeyboardInputManager.OnRotateInput -= UpdateFrameRotate;
        KeyboardInputManager.OnZoomInput -= UpdateFrameZoom;
    }

    void LateUpdate() {
        Move();
        // Rotate();
        Zoom();
    }

    void UpdateFrameMove(Vector3 value) {
        _frameMove += value;
    }

    void UpdateFrameRotate(float value) {
        _frameRotate += value;
    }

    void UpdateFrameZoom(float value) {
        _frameZoom += value;
    }

    void Move() {
        if (_frameMove == Vector3.zero) {
            return;
        }

        var modified = new Vector3(
            _frameMove.x * _lateralSpeed,
            _frameMove.y,
            _frameMove.z * _inOutSpeed
        );

        transform.position += transform.TransformDirection(modified) * Time.deltaTime;
        LockPositionInBounds();
        _frameMove = Vector3.zero;
    }

    void Rotate() {
        if (_frameRotate == 0f) {
            return;
        }

        transform.Rotate(Vector3.up, _frameRotate * Time.deltaTime * _rotateSpeed);
        _frameRotate = 0;
    }

    void Zoom() {
        if (_frameZoom < 0f) {
            _zoomStrategy.ZoomIn(
                _camera,
                Time.deltaTime * Math.Abs(_frameZoom) * _zoomSpeed,
                _nearZoomLimit
            );

            _frameZoom = 0;
        }
        else if (_frameZoom > 0f) {
            _zoomStrategy.ZoomOut(
                _camera,
                Time.deltaTime * _frameZoom * _zoomSpeed,
                _farZoomLimit
            );

            _frameZoom = 0;
        }
    }



    void LockPositionInBounds() {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, _minBounds.x, _maxBounds.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, _minBounds.y, _maxBounds.y)
        );
    }
}
