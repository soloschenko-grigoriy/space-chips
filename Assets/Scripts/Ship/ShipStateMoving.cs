using UnityEngine;

public class ShipStateMoving : ShipState {
    public ShipStateMoving(Ship ship) : base(ship) { }

    public override void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.M)) {
            _ship.IsMoving = false;
        };
    }
}
