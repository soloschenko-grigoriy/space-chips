using UnityEngine;

[RequireComponent(typeof(HexAgent))]
public class Ship : MonoBehaviour {
    public bool CanBeTargeted = false;
    public bool IsTarget = false;
    public bool CanMove => _canMove;
    public HexAgent HexAgent => _hexAgent;
    public Fleet Fleet => _fleet;

    [SerializeField] Color _idleColor = Color.white;
    [SerializeField] Color _awaitColor = Color.green;
    [SerializeField] Color _moveColor = Color.yellow;
    [SerializeField] Color _actColor = Color.red;
    [SerializeField] Color _canBeTargetColor = Color.blue;
    [SerializeField] Color _targetColor = Color.cyan;

    ShipStateMachina _stateMachina;
    Renderer _renderer;
    HexAgent _hexAgent;
    Fleet _fleet;
    HUD _hud;
    bool _isAwaiting, _isMoving, _canMove = false;
    Ship[] _targets;
    Ship _currentTarget;

    RaycastHit[] _raycastHits = new RaycastHit[100];

    void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
        _hexAgent = GetComponent<HexAgent>();
        _hud = FindObjectOfType<HUD>();

        var idleState = new ShipStateIdle(this);
        var awaitState = new ShipStateAwait(this);
        var moveState = new ShipStateMove(this);
        var actState = new ShipStateAct(this);

        idleState.Transitions = new ShipStateTransition[] {
            new ShipStateTransition(awaitState, () => _isAwaiting)
        };

        awaitState.Transitions = new ShipStateTransition[] {
            new ShipStateTransition(moveState, () => _isMoving),
            new ShipStateTransition(actState, () => !_isMoving && _currentTarget != null), // <------
            new ShipStateTransition(idleState, () => !_isAwaiting)
        };

        moveState.Transitions = new ShipStateTransition[] {
            new ShipStateTransition(awaitState, () => !_isMoving && _currentTarget == null),
            new ShipStateTransition(actState, () => !_isMoving && _currentTarget != null)
        };

        actState.Transitions = new ShipStateTransition[] {
            new ShipStateTransition(idleState, () => _currentTarget == null)
        };

        _stateMachina = new ShipStateMachina(new ShipState[] { idleState, awaitState, moveState, actState }, idleState);
    }

    void Update() {
        _stateMachina.Update();

        if (_isMoving) {
            _renderer.material.color = _moveColor;
        }
        else if (_currentTarget != null) {
            _renderer.material.color = _actColor;
        }
        else if (CanBeTargeted) {
            _renderer.material.color = _canBeTargetColor;
        }
        else if (_isAwaiting) {
            _renderer.material.color = _awaitColor;
        }
        else if (IsTarget) {
            _renderer.material.color = _targetColor;
        }
        else {
            _renderer.material.color = _idleColor;
        }
    }

    public Ship Spawn(Fleet fleet, HexCell hexCell) {
        _fleet = fleet;
        _hexAgent.Spawn(hexCell, fleet.Owner);
        _canMove = true;

        return this;
    }

    public void CheckForEnemiesInRange(int range) {
        var cells = HexAgent.HexGrid.FindAllOccupiedByEnemyInRange(HexAgent.CurrentCell.Coordinates, range);
        _targets = new Ship[cells.Length];

        for (var i = 0; i < cells.Length; i++) {
            _targets[i] = cells[i].OccupiedBy;
        }

        if (cells.Length > 0) {
            _hud.ActivateMeleeAttackButton();
        }
        else {
            _hud.DeactivateMeleeAttackButton();
        }
    }

    public void ClearTargets() {
        _hud.DeactivateMeleeAttackButton();

        for (var i = 0; i < _targets.Length; i++) {
            _targets[i].CanBeTargeted = false;
        }
    }

    public void Activate() {
        _isAwaiting = true;
        _canMove = true;
    }

    public void StartMovingTo(HexCell cell) {
        _hexAgent.SetDestination(cell);
        _isMoving = true;
        _canMove = false;
    }

    public void OnMovingDoneTo(HexCell cell) {
        _isMoving = false;
    }

    public void OnMeleeAttackSelected(bool value) {
        for (var i = 0; i < _targets.Length; i++) {
            _targets[i].CanBeTargeted = value;
        }
    }

    public void SetTarget(Ship target) {
        _currentTarget = target;
        target.IsTarget = true;

        // if Ship needs to move to target and actually can do so
        if (CanMove && HexAgent.CurrentCell.DistanceTo(target.HexAgent.CurrentCell) > 1) {
            var cell = target.HexAgent.CurrentCell.FindClosestNeighbor(HexAgent.CurrentCell);

            StartMovingTo(cell);
        }
    }

    public void SkipTurn() {
        _isAwaiting = false;
    }

    public void OnActionDone() {
        _isAwaiting = false;
        _currentTarget.IsTarget = false;
        _currentTarget = null;
    }
}
