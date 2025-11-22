using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public class LuminousSystem : ComponentSystemBase<Luminous, LuminousConfig>
    {
        public LuminousSystem Install()
        {
            Type = BehaviourType.Luminous;
            return this;
        }
    }
}
