public interface ILevelState
{
    // automatically gets called in the State machine. Allows you to delay flow if desired
    void Enter();
    // allows simulation of Update() method without a MonoBehaviour attached
    void Tick();
    // allows simulation of FixedUpdate() method without a MonoBehaviour attached
    void FixedTick();
    // automatically gets called in the State machine. Allows you to delay flow if desired
    void Exit();

    // THIS IS SO DUMB DON"T DO THIS
    int GetNumberOfEnemies();
}