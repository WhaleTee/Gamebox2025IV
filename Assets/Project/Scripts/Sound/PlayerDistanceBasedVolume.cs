using UnityEngine;

namespace Sound
{
    public class PlayerDistanceBasedVolume : DistanceBasedVolume
    {
        protected override void Update()
        {
            base.Update();
            if (target == null) target = GameObject.FindWithTag("Player")?.transform;
        }
    }
}