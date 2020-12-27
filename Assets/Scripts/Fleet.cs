using UnityEngine;

public enum FleetOwner {
    Player, AI
}

public class Fleet : MonoBehaviour {
    public FleetOwner Type { get => _type; }

    [SerializeField] Ship _shipPrefab = default;
    [SerializeField] Vector2Int[] _positions = default;
    [SerializeField] FleetOwner _type = default;

    Ship[] _ships;
    int _currentActive;

    public void SpawnOnGrid(HexGrid hexGrid) {
        _ships = new Ship[_positions.Length];
        _currentActive = 0;

        for (int i = 0; i < _positions.Length; i++) {
            var cell = hexGrid.FindBy(_positions[i]);

            if (cell == null) {
                Debug.LogError($"No cell with position {_positions[i]}");
                return;
            }

            _ships[i] = Instantiate(_shipPrefab).Spawn(this, cell);
        }

        ActivateNext();
    }

    public void ActivateNext() {
        if (_currentActive == _ships.Length) {
            _currentActive = 0;
        }

        _ships[_currentActive++].IsActive = true;
    }
}
