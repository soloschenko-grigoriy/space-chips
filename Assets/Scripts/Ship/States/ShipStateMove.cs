public class ShipStateMove : ShipState {
    public ShipStateMove(Ship ship) : base(ship) { }

    public override void OnEnter() {
        _ship.HexAgent.StartMoving(_ship.OnMovingDoneTo);
    }
}
