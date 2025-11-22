using UnityEngine;

namespace Combat.Weapon.State
{
    public class FireStart : StateBase
    {
        public override void Enter()
        {
            Fire();
        }

        private void Fire()
        {
            events.Start();
            events.Shoot();
            blackboard.NextShotTime = Time.time + stats.FireInterval;
        }
    }
}
