using UnityEngine;

namespace Misc.Trigger.Collisions
{
    public class Trigger : ObservableTriggerBase<Collider2D>
    {
        private void OnTriggerEnter2D(Collider2D collider) => Enter(collider);
        private void OnTriggerExit2D(Collider2D collider) => Exit(collider);
        private void OnTriggerStay2D(Collider2D collider) => Stay(collider);
    }
}
