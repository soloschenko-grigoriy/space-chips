public interface IState {
    ITransition[] Transitions { get; }

    void OnEnter();
    void OnUpdate();
    void OnExit();
}
