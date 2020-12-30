using UnityEngine;

public class ShipStateAwait : ShipState {
    public ShipStateAwait(Ship ship) : base(ship) { }
    RaycastHit[] _raycastHits = new RaycastHit[100];

    float timeToMakeAutoMove;
    float autoMoveDelay = 0.5f;
    bool autoMoveInProgress = false;

    public override void OnEnter() {
        HUD.OnSkipClick += _ship.SkipTurn;
        HUD.OnMeleeAttackClick += _ship.SelectAction;

        if (_ship.CanMove) {
            timeToMakeAutoMove = Time.time + autoMoveDelay;
            autoMoveInProgress = false;
            _ship.HexAgent.HighlightMovementRange();
        }
        else if (_ship.Fleet.Owner == FleetOwner.AI) {
            _ship.SkipTurn();
            return;
        } else if(_ship.Fleet.Owner == FleetOwner.Player){
            _ship.CheckEnemiesInMeleeRange();
        }
    }

    public override void OnUpdate() {
        switch (_ship.Fleet.Owner) {
            case FleetOwner.Player: WaitForInput(); break;
            case FleetOwner.AI: WaitForAutoMove(); break;
        }
    }

    public override void OnExit() {
        HUD.OnSkipClick -= _ship.SkipTurn;
        HUD.OnMeleeAttackClick -= _ship.SelectAction;
        _ship.HexAgent.HideMovementRange();
    }

    void WaitForInput() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(ray, _raycastHits);

            for (int i = 0; i < hits; i++) {
                var cell = _raycastHits[i].collider.GetComponentInParent<HexCell>();
                if (cell && cell.Type == HexCellType.Range) {
                    _ship.StartMovingTo(cell);
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

        _ship.StartMovingTo(_ship.HexAgent.GetRandomCellInRange());
        autoMoveInProgress = true;
    }

}
