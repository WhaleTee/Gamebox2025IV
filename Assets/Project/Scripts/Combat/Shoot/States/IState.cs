namespace Combat.Weapon.State
{
    public interface IState
    {
        void Enter();
        void Tick();
        void Exit();
    }
}
