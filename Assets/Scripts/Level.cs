using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    [SerializeField] HexGrid _hexGridPrefab = default;
    [SerializeField] Fleet[] _fleetPrefabs = default;

    HexGrid _hexGrid;
    Fleet[] _fleets;

    void Awake() {
        // Instantiate Fleets only after grid is ready
        _hexGrid = Instantiate(_hexGridPrefab);
        _hexGrid.Generate(() => SpawnFleet());
    }

    void SpawnFleet() {
        _fleets = new Fleet[_fleetPrefabs.Length];

        for (int i = 0; i < _fleetPrefabs.Length; i++) {
            _fleets[i] = Instantiate(_fleetPrefabs[i]);
            _fleets[i].SpawnOnGrid(_hexGrid);
        }
    }

}
