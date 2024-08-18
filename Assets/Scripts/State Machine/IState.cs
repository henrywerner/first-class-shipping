public interface IState
{
    // automatically gets called in the State machine. Allows you to delay flow if desired
    void Enter();
    // allows simulation of Update() method without a MonoBehaviour attached
    void Tick();
    // allows simulation of FixedUpdate() method without a MonoBehaviour attached
    void FixedTick();
    // automatically gets called in the State machine. Allows you to delay flow if desired
    void Exit();
}