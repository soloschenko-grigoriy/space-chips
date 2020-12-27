using System;

public interface ITransition {
    Func<bool> IsTriggered { get; }

    IState Target { get; }
}
