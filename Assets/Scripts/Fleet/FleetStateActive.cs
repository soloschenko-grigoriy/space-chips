using UnityEngine;

public class FleetStateActive : FleetState {
    public FleetStateActive(Fleet fleet) : base(fleet) { }

    public override void OnEnter() {
        _fleet.ActivateNextShip();
    }
}
