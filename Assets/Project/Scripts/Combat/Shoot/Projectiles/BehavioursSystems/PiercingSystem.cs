namespace Combat.Projectiles.Behaviours
{
    public class PiercingSystem : ComponentSystemBase<Piercing, PiercingConfig>
    {
        public PiercingSystem Install()
        {
            Type = BehaviourType.Piercing;
            return this;
        }
    }
}
