public abstract class FleetState : IState {
    public StateTransition[] Transitions { get; set; }

    protected Fleet _fleet;

    public FleetState(Fleet fleet) {
        _fleet = fleet;
    }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void OnUpdate() { }
}
