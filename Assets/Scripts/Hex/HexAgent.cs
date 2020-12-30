using System;
using System.Collections.Generic;
using UnityEngine;

public class HexAgent : MonoBehaviour {
    public delegate void OnComplete(HexCell cell);
    public HexCell CurrentCell => _currentCell;
    public HexGrid HexGrid => _hexGrid;
    public int MoveRange => _moveRange;

    [SerializeField] float _moveDuration = 0.3f;
    [SerializeField] int _moveRange = 2;

    HexGrid _hexGrid;
    Ship _ship;
    float _timeStarted;
    Vector3 _startPosition;
    List<HexCell> _path = new List<HexCell>();
    HexCell _currentCell;
    HexCell _nextCell;
    int _currentIndex;
    OnComplete _onComplete;
    FleetOwner _fleetOwner;

    void Awake() {
        _hexGrid = FindObjectOfType<HexGrid>();
        _ship = GetComponent<Ship>();
    }

    void Update() {
        if (!_currentCell || !_nextCell) {
            return;
        }

        ProcessMovementStep();
    }

    public void Spawn(HexCell cell, FleetOwner fleetOwner) {
        _fleetOwner = fleetOwner;
        _currentCell = cell;
        cell.Occupy(_ship);
        transform.position = _currentCell.transform.position;
    }

    public void SetDestination(HexCell cell) {
        // preserve path
        _path = new AStarSearch(_currentCell, cell)
            .Search()
            .Reconstruct();

        // highlight path
        for (int i = 0; i < _path.Count; i++) {
            _path[i].Type = HexCellType.Path;
        }
    }

    public void StartMoving(OnComplete onComplete) {
        _currentIndex = 0;
        _onComplete = onComplete;

        ContinueToNextCell();
    }

    public void HighlightMovementRange() {
        // first find all cells that can be in ranage (excluding obsticles)
        var cells = _hexGrid.FindAllEmptyInRange(_currentCell.Coordinates, _moveRange);

        for (int i = 0; i < cells.Length; i++) {
            // now build path to each one of them
            var path = new AStarSearch(_currentCell, cells[i])
                .Search()
                .Reconstruct();

            // and calculate how many "steps" it will actually take to get to them
            if (path.Count <= _moveRange) {
                cells[i].Type = HexCellType.Range;
            }
        }
    }

    public void HideMovementRange() {
        var cells = _hexGrid.FindAllEmptyInRange(_currentCell.Coordinates, _moveRange);
        for (int i = 0; i < cells.Length; i++) {
            if (cells[i].Type == HexCellType.Range) {
                cells[i].Type = HexCellType.Default;
            }
        }
    }

    public HexCell GetRandomCellInRange() {
        var cells = Array.FindAll(_hexGrid.Cells, (c) => c.Type == HexCellType.Range);
        var index = UnityEngine.Random.Range(0, cells.Length - 1);

        return cells[index];
    }

    void ContinueToNextCell() {
        _nextCell = _path[_currentIndex++];
        _timeStarted = Time.time;
        _startPosition = transform.position;
    }

    void ProcessMovementStep() {
        var progress = (Time.time - _timeStarted) / _moveDuration;
        if (progress >= 1) {
            _currentCell.Liberate();
            _currentCell = _nextCell;
            _currentCell.Occupy(_ship);

            transform.position = _currentCell.transform.position;

            if (_currentIndex < _path.Count) {
                ContinueToNextCell();
            }
            else {
                CleanupPath();
                _onComplete(_currentCell);
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

    void CleanupPath() {
        _nextCell = null;
        _path = new List<HexCell>();
    }
}
