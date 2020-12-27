using UnityEngine;

public class ShipStateActive : ShipState {
    public ShipStateActive(Ship ship) : base(ship) { }

    public override void OnExit() {
        _ship.IsActive = false;
    }

    public override void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.M)) {
            _ship.IsMoving = true;
        };
    }
}
