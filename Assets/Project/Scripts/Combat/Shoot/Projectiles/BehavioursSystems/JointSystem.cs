using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public class JointSystem : ComponentSystemBase<Joint, JointConfig>
    {
        public JointSystem Install()
        {
            Type = BehaviourType.Joint;
            return this;
        }
    }
}
