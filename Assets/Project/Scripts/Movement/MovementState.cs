namespace Movement
{
    public abstract class MovementState : IUpdatable, IFixedUpdatable, IExitable
    {
        public abstract void Update();

        public abstract void FixedUpdate();

        public virtual void Exit() { }
    }
}