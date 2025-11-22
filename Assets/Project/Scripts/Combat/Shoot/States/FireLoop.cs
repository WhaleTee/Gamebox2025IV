using UnityEngine;

namespace Combat.Weapon.State
{
    public class FireLoop : StateBase
    {
        public override void Tick()
        {
            Fire();
        }

        private void Fire()
        {
            if (Time.time < blackboard.NextShotTime)
                return;

            blackboard.NextShotTime = Time.time + stats.FireInterval;
            events.Continue();
            events.Shoot();
        }
    }
}
