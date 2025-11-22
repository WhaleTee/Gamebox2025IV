using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class UpdateRunner: MonoBehaviour, IUpdateRunner
    {
        private List<IUpdatable> updatables = new();

        public void Register(IUpdatable updatable) => updatables.Add(updatable);

        public void Unregister(IUpdatable updatable) => updatables.Remove(updatable);

        private void Update()
        {
            foreach (var updatable in updatables)
                if (updatable.Enabled)
                    updatable.Update(Time.deltaTime);
        }
    }
}
