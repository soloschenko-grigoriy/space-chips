using UnityEngine;

public class Fleet : MonoBehaviour {
    [SerializeField] Ship _shipPrefab = default;
    [SerializeField] Vector2Int[] _positions = default;

    Ship[] _ships;

    public void SpawnOnGrid(HexGrid hexGrid) {
        _ships = new Ship[_positions.Length];

        for (int i = 0; i < _positions.Length; i++) {
            var cell = hexGrid.FindBy(_positions[i]);

            if (cell == null) {
                Debug.LogError($"No cell with position {_positions[i]}");
                return;
            }

            _ships[i] = Instantiate(_shipPrefab);
            _ships[i].HexAgent.Spawn(cell);
        }
    }
}
