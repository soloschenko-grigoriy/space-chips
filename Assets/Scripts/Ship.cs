using UnityEngine;

[RequireComponent(typeof(HexAgent))]
public class Ship : MonoBehaviour {
    public bool IsActive, IsMoving = false;
    public HexAgent HexAgent { get; private set; }
    public Fleet Fleet { get; private set; }

    [SerializeField] Color _inactiveColor = Color.white;
    [SerializeField] Color _activeColor = Color.green;
    [SerializeField] Color _movingColor = Color.red;

    ShipStateMachina _stateMachina;
    Renderer _renderer;

    void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
        HexAgent = GetComponent<HexAgent>();

        var idleState = new ShipStateIdle(this);
        var activeState = new ShipStateActive(this);
        var movingState = new ShipStateMoving(this);

        var idleToActiveTransition = new ShipStateTransition(activeState, () => IsActive);
        var activeToMovingTransition = new ShipStateTransition(movingState, () => IsMoving);
        var movingToIdleTransition = new ShipStateTransition(idleState, () => !IsMoving);

        idleState.Transitions = new ShipStateTransition[] { idleToActiveTransition };
        activeState.Transitions = new ShipStateTransition[] { activeToMovingTransition };
        movingState.Transitions = new ShipStateTransition[] { movingToIdleTransition };

        _stateMachina = new ShipStateMachina(new ShipState[] { idleState, activeState, movingState }, idleState);
    }

    void Update() {
        _stateMachina.Update();

        if (IsActive) {
            _renderer.material.color = _activeColor;
        }
        else if (IsMoving) {
            _renderer.material.color = _movingColor;
        }
        else {
            _renderer.material.color = _inactiveColor;
        }
    }

    public Ship Spawn(Fleet fleet, HexCell hexCell) {
        Fleet = fleet;
        HexAgent.Spawn(hexCell);

        return this;
    }
}
