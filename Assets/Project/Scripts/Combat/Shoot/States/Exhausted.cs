using UnityEngine;

namespace Combat.Weapon.State
{
    public class Exhausted : StateBase
    {
        public override void Tick()
        {
            Debug.Log("Exhausted");
            if (blackboard.Fire)
                blackboard.Fire = false;
        }
    }
}
