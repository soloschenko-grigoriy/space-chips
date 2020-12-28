using System;

public abstract class StateTransition {
    public Func<bool> IsTriggered { get; }

    public IState Target { get; }

    public StateTransition(IState target, Func<bool> isTriggered) {
        Target = target;
        IsTriggered = isTriggered;
    }
}
