public class StateMachina {
    public IState[] States;

    IState _currentState;

    public StateMachina(IState[] states, IState initState) {
        States = states;
        _currentState = initState;
    }

    public void Update() {
        for (int i = 0; i < _currentState.Transitions.Length; i++) {
            if (_currentState.Transitions[i].IsTriggered()) {
                _currentState.OnExit();
                _currentState = _currentState.Transitions[i].Target;
                _currentState.OnEnter();

                break;
            }
        }

        _currentState.OnUpdate();
    }
}
