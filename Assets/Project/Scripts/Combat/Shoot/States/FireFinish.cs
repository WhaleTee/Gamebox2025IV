namespace Combat.Weapon.State
{
    public class FireFinish : StateBase
    {
        public override void Enter()
        {
            events.Finish();
        }
    }
}
