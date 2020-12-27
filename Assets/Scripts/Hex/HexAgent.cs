using System;
using System.Collections.Generic;
using UnityEngine;

public class HexAgent : MonoBehaviour {
    public delegate void OnComplete(HexCell cell);

    [SerializeField] float _moveDuration = 0.3f;
    [SerializeField] int _moveRange = 2;

    HexGrid _hexGrid;
    float _timeStarted;
    Vector3 _startPosition;
    List<HexCell> _path = new List<HexCell>();
    HexCell _currentCell;
    HexCell _nextCell;
    int _currentIndex;
    OnComplete _onComplete;

    void Awake() {
        _hexGrid = FindObjectOfType<HexGrid>();
    }

    void Update() {
        if (!_currentCell || !_nextCell) {
            return;
        }

        ProcessMovementStep();
    }

    public void Spawn(HexCell cell) {
        _currentCell = cell;
        cell.Type = HexCellHighlightType.Occupied;
        transform.position = _currentCell.transform.position;
    }

    public void SetDestination(HexCell cell) {
        var path = new AStarSearch(_currentCell, cell);

        foreach (var item in path.cameFrom.Values) {
            if (item && !_path.Contains(item)) {
                _path.Add(item);
                item.Type = HexCellHighlightType.Path;
            }
        }

        _path.Add(cell);
        cell.Type = HexCellHighlightType.Path;
    }

    public void StartMoving(OnComplete onComplete) {
        _currentIndex = 0;
        _onComplete = onComplete;
        ContinueToNextCell();
    }

    public void HighlightMovementRange() {
        _hexGrid.SetTypeInRange(_currentCell, _moveRange, HexCellHighlightType.Range);
    }

    public void HideMovementRange() {
        _hexGrid.SetTypeInRange(_currentCell, _moveRange, HexCellHighlightType.Default);
    }

    public HexCell GetRandomCellInRange() {
        var cells = Array.FindAll(_hexGrid.Cells, (c) => c.Type == HexCellHighlightType.Range);
        var index = UnityEngine.Random.Range(0, cells.Length - 1);

        return cells[index];
    }
    
    void ContinueToNextCell() {
        _nextCell = _path[_currentIndex++];
        _timeStarted = Time.time;
        _startPosition = transform.position;
    }

    void ProcessMovementStep() {
        var progress = (Time.time - _timeStarted) / this._moveDuration;
        if (progress >= 1) {
            _currentCell.Type = HexCellHighlightType.Default;
            _currentCell = _nextCell;
            _currentCell.Type = HexCellHighlightType.Occupied;

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
