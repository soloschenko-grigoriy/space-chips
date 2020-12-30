using UnityEngine;

public class FleetStateIdle : FleetState {
    public FleetStateIdle(Fleet fleet) : base(fleet) { }

    public override void OnEnter() {
        _fleet.OppositeFleet.IsActive = true;
    }
}
