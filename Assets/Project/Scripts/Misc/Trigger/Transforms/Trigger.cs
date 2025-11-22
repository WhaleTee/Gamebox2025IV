using UnityEngine;

namespace Misc.Trigger.Transforms
{
    public class Trigger : ObservableTriggerBase<Transform>
    {
        private void OnTriggerEnter2D(Collider2D collider) => Enter(collider.transform);
        private void OnTriggerExit2D(Collider2D collider) => Exit(collider.transform);
        private void OnTriggerStay2D(Collider2D collider) => Stay(collider.transform);
    }
}
