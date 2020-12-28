using System;

public class FleetStateTransition : StateTransition {
    public FleetStateTransition(FleetState target, Func<bool> isTriggered) : base(target, isTriggered) { }
}
