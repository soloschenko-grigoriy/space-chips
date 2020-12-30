using UnityEngine;

public class ShipStateAct : ShipState {
    public ShipStateAct(Ship ship) : base(ship) { }

    // fake await, update with real calculations
    float timeToMakeAutoMove;
    float autoMoveDelay = 1f;
    bool autoMoveInProgress = false;

    public override void OnEnter() {
        timeToMakeAutoMove = Time.time + autoMoveDelay;
        autoMoveInProgress = false;
    }

    public override void OnUpdate() {
        if (autoMoveInProgress) {
            return;
        }

        if (Time.time < timeToMakeAutoMove) {
            return;
        }

        autoMoveInProgress = true;
        _ship.OnActionDone();
    }
}
