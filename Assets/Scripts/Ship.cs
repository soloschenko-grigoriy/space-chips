using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    [SerializeField] HexGrid hexGrid = default;
    [SerializeField] float _moveDuration = 0.3f;

    RaycastHit[] _raycastHits = new RaycastHit[100];
    float _timeStarted;
    Vector3 _startPosition;
    List<HexCell> _path = new List<HexCell>();
    HexCell _currentCell;
    HexCell _nextCell;
    int _currentIndex;

    void Start() {
        _currentCell = hexGrid.Cells[0];
        transform.position = _currentCell.transform.position;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleInput();
        }

        if (!_currentCell || !_nextCell) {
            return;
        }

        var progress = (Time.time - _timeStarted) / this._moveDuration;
        if (progress >= 1) {
            _currentCell = _nextCell;
            transform.position = _currentCell.transform.position;

            if (_currentIndex < _path.Count) {
                S();
            }
            else {
                CleanupPath();
            }

        }
        else {
            transform.position = Vector3.Lerp(
                this._startPosition,
                _nextCell.transform.position,
                progress
            );
        }
    }

    void S() {
        _nextCell = _path[_currentIndex++];
        _timeStarted = Time.time;
        _startPosition = transform.position;
    }

    void Spawn(HexCell at) {
        enabled = true;
        _currentCell = at;
        _currentIndex = 0;

        S();
    }

    void CleanupPath() {
        _nextCell = null;
        _path = new List<HexCell>();
        foreach (var cell in hexGrid.Cells) {
            cell.IsInPath = false;
        }
    }

    void HandleInput() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hits = Physics.RaycastNonAlloc(ray, _raycastHits);



        for (int i = 0; i < hits; i++) {
            var cell = _raycastHits[i].collider.GetComponentInParent<HexCell>();
            if (cell) {
                StartPath(cell);
                break;
            }
        }
    }

    void StartPath(HexCell cell) {
        var path = new AStarSearch(_currentCell, cell);

        foreach (var item in path.cameFrom.Values) {
            if (item && !_path.Contains(item)) {
                _path.Add(item);
                item.IsInPath = true;
            }
        }
        _path.Add(cell);
        cell.IsInPath = true;
        // HighlightInRange(cell, 3);
        // cell.ToggleIsActive();
        Spawn(cell);
        // hexGrid.HighlightPath(currentCell, cell);
    }
}
