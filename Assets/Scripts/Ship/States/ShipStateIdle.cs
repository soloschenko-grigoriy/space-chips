public class ShipStateIdle : ShipState {

    public ShipStateIdle(Ship ship) : base(ship) { }

    public override void OnEnter() {
        _ship.Fleet.ActivateNextShip();
    }
}
