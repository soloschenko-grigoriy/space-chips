using UnityEngine;

[RequireComponent(typeof(HexAgent))]
public class Ship : MonoBehaviour {
    public bool IsActive, IsMoving = false;
    public HexAgent HexAgent { get; private set; }

    [SerializeField] Color inactiveColor = Color.white;
    [SerializeField] Color activeColor = Color.green;
    [SerializeField] Color movingColor = Color.red;

    StateMachina _stateMachina;
    Renderer _renderer;

    void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
        HexAgent = GetComponent<HexAgent>();

        var idleState = new ShipStateIdle(this);
        var activeState = new ShipStateActive(this);
        var movingState = new ShipStateMoving(this);

        var idleToActiveTransition = new ShipTransition(activeState, () => IsActive);
        var activeToMovingTransition = new ShipTransition(movingState, () => IsMoving);
        var movingToIdleTransition = new ShipTransition(idleState, () => !IsMoving);

        idleState.Transitions = new ShipTransition[] { idleToActiveTransition };
        activeState.Transitions = new ShipTransition[] { activeToMovingTransition };
        movingState.Transitions = new ShipTransition[] { movingToIdleTransition };

        _stateMachina = new StateMachina(new IState[] { idleState, activeState, movingState }, idleState);
    }

    void Update() {
        _stateMachina.Update();

        if (IsActive) {
            _renderer.material.color = activeColor;
        }
        else if (IsMoving) {
            _renderer.material.color = movingColor;
        }
        else {
            _renderer.material.color = inactiveColor;
        }
    }
}
