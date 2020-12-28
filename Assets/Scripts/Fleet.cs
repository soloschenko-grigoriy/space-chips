using UnityEngine;

public enum FleetOwner {
    Player, AI
}

public class Fleet : MonoBehaviour {
    public FleetOwner Owner { get => _type; }
    public bool IsActive { get; set; }
    public Fleet OppositeFleet { get; private set; }

    [SerializeField] Ship _shipPrefab = default;
    [SerializeField] Vector2Int[] _positions = default;
    [SerializeField] FleetOwner _type = default;

    Ship[] _ships;
    int _currentActive;

    FleetStateMachina _stateMachina;

    void Awake() {
        var idleStateF = new FleetStateIdle(this);
        var activeStateF = new FleetStateActive(this);

        var idleToActiveTransitionF = new FleetStateTransition(activeStateF, () => IsActive);
        var activeToIdleTransitionF = new FleetStateTransition(idleStateF, () => !IsActive);

        idleStateF.Transitions = new FleetStateTransition[] { idleToActiveTransitionF };
        activeStateF.Transitions = new FleetStateTransition[] { activeToIdleTransitionF };

        _stateMachina = new FleetStateMachina(new FleetState[] { idleStateF, activeStateF }, idleStateF);
    }

    void Update() {
        _stateMachina.Update();
    }

    public void SpawnOnGrid(HexGrid hexGrid, Fleet oppositeFleet) {
        OppositeFleet = oppositeFleet;
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
    }

    public void ActivateNextShip() {
        if (_currentActive == _ships.Length) {
            _currentActive = 0;
            IsActive = false;

            return;
        }

        _ships[_currentActive++].IsActive = true;
    }
}
