using System.Collections.Generic;
using Reflex.Attributes;
using Misc;

namespace Combat.Projectiles.Behaviours
{
    public class BehavioursSystem : IUpdatable, IInitializable, IInjectable
    {
        public DefaultSystem DefaultSystem => defaultSystem;
        [Inject] private IUpdateRunner updateRunner;
        private DefaultSystem defaultSystem;
        private List<IComponentSystem> systems;
        private static int currentId = 0;

        public bool Enabled { get; private set; }

        public BehavioursSystem()
        {
            defaultSystem = new();
            systems = new()
            {
                //new StickySystem().Install(defaultSystem),
                //new LuminousSystem(),
                //new PiercingSystem(),
                //new RicochetSystem().Install(defaultSystem),
                //new HomingSystem(),
                //new BoomerangSystem(),
                //new JointSystem(),
                //new ChangeTargetSystem()
            };
        }

        public void Set(int id, DefaultBehaviour defaultBehaviour)
        {
            defaultSystem.SetComponent(id, defaultBehaviour);
        }

        public DefaultBehaviour Get(int id)
        {
            return defaultSystem.GetComponent(id);
        }

        public void Reset(int id)
        {
            foreach (var system in systems)
                system.Reset(id);
        }

        public void Register(Projectile projectile)
        {
            int id = currentId++;

            projectile.SetID(id);
            ProjectileConfig config = projectile.Config;

            defaultSystem.Register(id, config, new(projectile, projectile.Rigidbody, config.DefaultConfig.Speed));
            defaultSystem.OnDefEvents(id, Unregister);

            foreach (var system in systems)
                system.Register(id, config);
        }

        public void Unregister(int id)
        {
            foreach (var system in systems)
                system.Unregister(id);
            defaultSystem.Unregister(id);
        }

        public void Tick(float deltaTime)
        {
            defaultSystem.Tick(deltaTime);

            foreach (var system in systems)
                system.Tick(deltaTime);
        }

        public void Update(float deltaTime) => Tick(deltaTime);

        public void SetActive(bool value)
        {
            Enabled = value;
            if (value)
                updateRunner.Register(this);
            else
                updateRunner.Unregister(this);
        }

        public void Initialize() => SetActive(true);
    }
}
