using System;

public class ShipStateTransition : StateTransition {
    public ShipStateTransition(ShipState target, Func<bool> isTriggered) : base(target, isTriggered) { }
}
