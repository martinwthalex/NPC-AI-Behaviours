public interface IState
{
    void Enter();
    void Update();
    void Exit();
}
// This interface defines the structure for a state in a state machine