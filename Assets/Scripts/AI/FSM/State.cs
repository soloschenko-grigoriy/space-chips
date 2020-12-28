public interface IState {
    StateTransition[] Transitions { get; }

    void OnEnter();
    void OnUpdate();
    void OnExit();
}
