using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    [SerializeField] HexGrid _hexGridPrefab = default;
    [SerializeField] Fleet _fleetPrefab = default;

    HexGrid _hexGrid;
    Fleet _playerFleet;

    void Awake() {
        // Instantiate Fleets only after grid is ready
        _hexGrid = Instantiate(_hexGridPrefab);
        _hexGrid.Generate(() => SpawnFleet());
    }

    void SpawnFleet() {
        _playerFleet = Instantiate(_fleetPrefab);
        _playerFleet.SpawnOnGrid(_hexGrid);
    }

}
