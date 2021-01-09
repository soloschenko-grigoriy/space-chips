using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HexAgent))]
public class Ship : MonoBehaviour {
    public bool CanBeTargeted {
        get => _canBeTargeted;
        set {
            _canBeTargeted = value;

            if (value) {
                _warMachine.ShowStats();
            }
            else if (!IsTarget) {
                _warMachine.HideStats();
            }
        }
    }
    public bool IsTarget {
        get => _isTarget;
        set {
            _isTarget = value;
            if (value) {
                _warMachine.ShowMethodSelection();
            }
            else {
                _warMachine.HideStats();
                _warMachine.HideMethodSelection();
            }
        }
    }
    public bool CanMove => _canMove;
    public HexAgent HexAgent => _hexAgent;
    public Fleet Fleet => _fleet;
    public WarMachine WarMachine => _warMachine;

    [SerializeField] int _rangeAttackRange = 7;
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
    bool _isAwaiting, _isMoving, _canMove, _canBeTargeted, _isTarget, _isActing = false;
    Ship[] _meleeTargets;
    Ship[] _rangeTargets;
    Ship _currentTarget;
    AttackType _attackType;
    WarMachine _warMachine;
    AttackMethod _attackMethod;

    void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
        _hexAgent = GetComponent<HexAgent>();
        _warMachine = GetComponent<WarMachine>();
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
            new ShipStateTransition(awaitState, () => !_isMoving && !_isActing),
            new ShipStateTransition(actState, () => !_isMoving && _isActing)
        };

        actState.Transitions = new ShipStateTransition[] {
            new ShipStateTransition(idleState, () => !_isActing)
        };
        _stateMachina = new ShipStateMachina(new ShipState[] { idleState, awaitState, moveState, actState }, idleState);
    }

    void Update() {
        _stateMachina.Update();

        if (_stateMachina.CurrentState is ShipStateMove) {
            _renderer.material.color = _moveColor;
        }
        else if (_stateMachina.CurrentState is ShipStateAct) {
            _renderer.material.color = _actColor;
        }
        else if (CanBeTargeted) {
            _renderer.material.color = _canBeTargetColor;
        }
        else if (_stateMachina.CurrentState is ShipStateAwait) {
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

    public void CheckForEnemies() {
        var meleeCells = HexAgent.HexGrid.FindAllOccupiedByEnemyInRange(HexAgent.CurrentCell.Coordinates, CanMove ? HexAgent.MoveRange + 1 : 1);
        var rangeCells = HexAgent.HexGrid.FindAllOccupiedByEnemyInRange(HexAgent.CurrentCell.Coordinates, _rangeAttackRange);

        if (meleeCells.Length > 0) {
            _hud.ActivateMeleeAttackButton();
        }
        else {
            _hud.DeactivateMeleeAttackButton();
        }

        if (rangeCells.Length > 0) {
            _hud.ActivateRangeAttackButton();
        }
        else {
            _hud.DeactivateRangeAttackButton();
        }

        _meleeTargets = new Ship[meleeCells.Length];
        _rangeTargets = new Ship[rangeCells.Length];
        for (var i = 0; i < meleeCells.Length; i++) {
            _meleeTargets[i] = meleeCells[i].OccupiedBy;
        }

        for (var i = 0; i < rangeCells.Length; i++) {
            _rangeTargets[i] = rangeCells[i].OccupiedBy;

        }
    }

    public void ClearTargets() {
        _hud.DeactivateMeleeAttackButton();
        _hud.DeactivateRangeAttackButton();

        if (_currentTarget != null) {
            _currentTarget.WarMachine.ClearPreviewDamage();
            _currentTarget.WarMachine.HideMethodSelection();
        }

        if (_meleeTargets != null) {
            for (var i = 0; i < _meleeTargets.Length; i++) {
                _meleeTargets[i].CanBeTargeted = false;
            }
        }

        if (_rangeTargets != null) {
            for (var i = 0; i < _rangeTargets.Length; i++) {
                _rangeTargets[i].CanBeTargeted = false;
            }
        }
    }

    public void Activate() {
        _isAwaiting = true;
        _canMove = true;
        _warMachine.ShowStats();
    }

    public void StartMovingTo(HexCell cell) {
        _hexAgent.SetDestination(cell);
        _isMoving = true;
        _canMove = false;
    }

    public void OnMovingDoneTo(HexCell cell) {
        _isMoving = false;
    }

    public void OnAttackSelected(AttackType type, bool value) {
        switch (type) {
            case AttackType.Melee: OnMeleeAttackSelected(value); break;
            case AttackType.Range: OnRangeAttackSelected(value); break;
        }
    }

    public void SetTarget(Ship target) {
        if (_currentTarget != null) {
            return;
        }

        _currentTarget = target;
        target.IsTarget = true;
    }

    public void Execute() {
        _isActing = true;

        if (_attackType != AttackType.Melee) {
            return;
        }

        // if Ship needs to move to target and actually can do so
        if (CanMove && HexAgent.CurrentCell.DistanceTo(_currentTarget.HexAgent.CurrentCell) > 1) {
            StartMovingTo(_currentTarget.HexAgent.CurrentCell.FindClosestNeighbor(HexAgent.CurrentCell));
        }

    }

    public void SkipTurn() {
        _isAwaiting = false;
        _warMachine.HideStats();
        _warMachine.HideMethodSelection();
    }

    public void OnActionDone() {
        _currentTarget.WarMachine.TakeDamage(_attackMethod, _warMachine.Power);

        StartCoroutine(Cleanup());
    }

    IEnumerator Cleanup() {
        yield return new WaitForSeconds(0.5f);
        _currentTarget.IsTarget = false;
        _currentTarget = null;

        _warMachine.HideStats();

        _isAwaiting = false;
        _isActing = false;

    }

    public void AllowSkip() {
        _hud.ActivateSkipButton();
    }

    public void DisableSkip() {
        _hud.DeactivateSkipButton();
    }

    public void EstimateDamage(AttackMethod method) {
        _attackMethod = method;
        _currentTarget.WarMachine.PreviewDamage(_attackMethod, _warMachine.Power);
    }

    void OnMeleeAttackSelected(bool value) {
        _attackType = AttackType.Melee;
        for (var i = 0; i < _meleeTargets.Length; i++) {
            _meleeTargets[i].CanBeTargeted = value;
        }
    }

    void OnRangeAttackSelected(bool value) {
        _attackType = AttackType.Range;
        for (var i = 0; i < _rangeTargets.Length; i++) {
            if (_rangeTargets[i] != null) {
                _rangeTargets[i].CanBeTargeted = value;
            }

        }
    }
}
