namespace Combat.Projectiles.Behaviours
{
    public class BoomerangSystem : ComponentSystemBase<Boomerang, BoomerangConfig>
    {
        public BoomerangSystem Install()
        {
            Type = BehaviourType.Boomerang;
            return this;
        }

        public override void Register(int id, ProjectileConfig config, Boomerang component = default)
        {
            base.Register(id, config, component);
        }

        public override void Tick(float deltaTime)
        {
            var keys = components.Keys;
            foreach (var key in keys)
            {
                var comp = components[key];
                var config = configs[key];

                comp.TimeSinceStart += deltaTime;
                components[key] = comp;
            }
        }
    }
}
