namespace Combat.Projectiles.Behaviours
{
    public class ChangeTargetSystem : ComponentSystemBase<ChangeTarget, ChangeTargetConfig>
    {
        public ChangeTargetSystem Install()
        {
            Type = BehaviourType.ChangeTarget;
            return this;
        }
    }
}
