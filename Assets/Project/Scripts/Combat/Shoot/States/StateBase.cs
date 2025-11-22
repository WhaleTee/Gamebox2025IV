namespace Combat.Weapon.State
{
    public class StateBase: IState
    {
        protected WeaponStats stats;
        protected Events events;
        protected Blackboard blackboard;

        public void Install(WeaponStats stats, Events events, Blackboard blackboard)
        {
            this.stats = stats;
            this.events = events;
            this.blackboard = blackboard;
        }

        public virtual void Enter()
        { }
        public virtual void Exit()
        { }
        public virtual void Tick()
        { }
    }
}
