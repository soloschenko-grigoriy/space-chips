public abstract class ShipState : IState {
    public ITransition[] Transitions { get; set; }

    protected Ship _ship;

    public ShipState(Ship ship) {
        _ship = ship;
    }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void OnUpdate() { }
}
