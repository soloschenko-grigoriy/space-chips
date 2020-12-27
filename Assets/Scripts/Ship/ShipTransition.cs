using System;

public class ShipTransition : ITransition {
    public IState Target { get; }
    public Func<bool> IsTriggered { get; }
    public ShipTransition(IState target, Func<bool> isTriggered) {
        Target = target;
        IsTriggered = isTriggered;
    }
}
