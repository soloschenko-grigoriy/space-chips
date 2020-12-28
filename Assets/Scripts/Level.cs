using System;
using UnityEngine;

public class Level : MonoBehaviour {
    [SerializeField] HexGrid _hexGridPrefab = default;
    [SerializeField] Fleet _playerFleetPrefab = default;
    [SerializeField] Fleet _enemyFleetPrefab = default;

    HexGrid _hexGrid;

    void Awake() {
        // Instantiate Fleets only after grid is ready
        _hexGrid = Instantiate(_hexGridPrefab);
        _hexGrid.Generate(() => SpawnFleet());
    }

    void SpawnFleet() {
        var playerFleet = Instantiate(_playerFleetPrefab);
        var enemyFleet = Instantiate(_enemyFleetPrefab);

        playerFleet.SpawnOnGrid(_hexGrid, enemyFleet);
        enemyFleet.SpawnOnGrid(_hexGrid, playerFleet);

        playerFleet.IsActive = true;
    }
}
