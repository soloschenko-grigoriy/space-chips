using UnityEngine;

[RequireComponent(typeof(HexAgent))]
public class Ship : MonoBehaviour {
    [SerializeField] Color inactiveColor = Color.white;
    [SerializeField] Color activeColor = Color.green;
    [SerializeField] Color movingColor = Color.red;

    StateMachina _stateMachina;
    HexAgent _hexAgent;
    RaycastHit[] _raycastHits = new RaycastHit[100];
    Renderer _renderer;

    public bool IsActive, IsMoving = false;

    void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
        _hexAgent = GetComponent<HexAgent>();

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

        if (Input.GetMouseButtonDown(0)) {
            HandleInput();
        }

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

    void HandleInput() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hits = Physics.RaycastNonAlloc(ray, _raycastHits);

        for (int i = 0; i < hits; i++) {
            var cell = _raycastHits[i].collider.GetComponentInParent<HexCell>();
            if (cell) {
                _hexAgent.SetDestination(cell);
                break;
            }
        }
    }
}
