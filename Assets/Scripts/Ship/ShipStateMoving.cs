using UnityEngine;

public class ShipStateMoving : ShipState {
    public ShipStateMoving(Ship ship) : base(ship) { }

    public override void OnEnter() {
        _ship.HexAgent.StartMoving((HexCell cell) => _ship.IsMoving = false);
    }
}
