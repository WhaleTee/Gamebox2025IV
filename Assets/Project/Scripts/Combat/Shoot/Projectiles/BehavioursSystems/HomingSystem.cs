using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public class HomingSystem : ComponentSystemBase<Homing, HomingConfig>
    {
        public HomingSystem Install()
        {
            Type = BehaviourType.Homing;
            return this;
        }
    }
}