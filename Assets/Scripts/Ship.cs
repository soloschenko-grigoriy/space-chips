using UnityEngine;

[RequireComponent(typeof(HexAgent))]
public class Ship : MonoBehaviour {
    public bool CanMove => _canMove;
    public HexAgent HexAgent => _hexAgent;
    public Fleet Fleet => _fleet;

    [SerializeField] Color _idleColor = Color.white;
    [SerializeField] Color _awaitColor = Color.green;
    [SerializeField] Color _moveColor = Color.yellow;
    [SerializeField] Color _actColor = Color.red;

    ShipStateMachina _stateMachina;
    Renderer _renderer;
    HexAgent _hexAgent;
    Fleet _fleet;
    HUD _hud;
    bool _isAwaiting, _isMoving, _isActing, _canMove = false;

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
            new ShipStateTransition(actState, () => _isActing),
            new ShipStateTransition(idleState, () => !_isAwaiting)
        };

        moveState.Transitions = new ShipStateTransition[] {
            new ShipStateTransition(awaitState, () => !_isMoving)
        };

        actState.Transitions = new ShipStateTransition[] {
            new ShipStateTransition(idleState, () => !_isActing)
        };

        _stateMachina = new ShipStateMachina(new ShipState[] { idleState, awaitState, moveState, actState }, idleState);
    }

    void Update() {
        _stateMachina.Update();

        if (_isMoving) {
            _renderer.material.color = _moveColor;
        }
        else if (_isActing) {
            _renderer.material.color = _actColor;
        }
        else if (_isAwaiting) {
            _renderer.material.color = _awaitColor;
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

    public void CheckEnemiesInMeleeRange() {
        var cells = _hexAgent.HexGrid.FindAllOccupiedByEnemyInRange(_hexAgent.CurrentCell.Coordinates, 1);
        if (cells.Length > 0) {
            _hud.ActivateMeleeAttackButton();
        }
        else {
            _hud.DeactivateMeleeAttackButton();
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

    public void SelectAction() {
        _isActing = true;
    }

    public void SkipTurn() {
        _isAwaiting = false;
    }

    public void OnActionDone() {
        _isAwaiting = false;
        _isActing = false;
    }
}
