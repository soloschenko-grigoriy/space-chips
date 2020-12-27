using UnityEngine;

public class ShipStateIdle : ShipState {

    public ShipStateIdle(Ship ship) : base(ship) { }

    public override void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.A)) {
            _ship.IsActive = true;
        }
    }
}
