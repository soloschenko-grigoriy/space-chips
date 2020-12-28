using UnityEngine;

public class ShipStateActive : ShipState {
    public ShipStateActive(Ship ship) : base(ship) { }
    RaycastHit[] _raycastHits = new RaycastHit[100];

    float timeToMakeAutoMove;
    float autoMoveDelay = 0.5f;
    bool autoMoveInProgress = false;

    public override void OnEnter() {
        timeToMakeAutoMove = Time.time + autoMoveDelay;
        autoMoveInProgress = false;
        _ship.HexAgent.HighlightMovementRange();
    }

    public override void OnExit() {
        _ship.IsActive = false;
    }

    public override void OnUpdate() {
        switch (_ship.Fleet.Owner) {
            case FleetOwner.Player: WaitForInput(); break;
            case FleetOwner.AI: WaitForAutoMove(); break;
        }
    }

    void WaitForInput() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(ray, _raycastHits);

            for (int i = 0; i < hits; i++) {
                var cell = _raycastHits[i].collider.GetComponentInParent<HexCell>();
                if (cell && cell.Type == HexCellHighlightType.Range) {
                    SetDestination(cell);
                    break;
                }
            }
        }
    }

    void WaitForAutoMove() {
        if (autoMoveInProgress) {
            return;
        }

        if (Time.time < timeToMakeAutoMove) {
            return;
        }

        SetDestination(_ship.HexAgent.GetRandomCellInRange());
        autoMoveInProgress = true;
    }

    void SetDestination(HexCell cell) {
        _ship.HexAgent.HideMovementRange();
        _ship.HexAgent.SetDestination(cell);
        _ship.IsMoving = true;
    }
}
