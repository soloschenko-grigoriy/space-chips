using UnityEngine;

public class ShipStateActive : ShipState {
    public ShipStateActive(Ship ship) : base(ship) { }

    RaycastHit[] _raycastHits = new RaycastHit[100];

    public override void OnEnter() {
        _ship.HexAgent.HighlightMovementRange();
    }

    public override void OnExit() {
        _ship.IsActive = false;
    }

    public override void OnUpdate() {
        if (Input.GetMouseButtonDown(0)) {
            HandleInput();
        }
    }

    void HandleInput() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hits = Physics.RaycastNonAlloc(ray, _raycastHits);

        for (int i = 0; i < hits; i++) {
            var cell = _raycastHits[i].collider.GetComponentInParent<HexCell>();
            if (cell && cell.Type == HexCellHighlightType.Range) {
                _ship.HexAgent.HideMovementRange();
                _ship.HexAgent.SetDestination(cell);
                _ship.IsMoving = true;
                break;
            }
        }
    }
}
