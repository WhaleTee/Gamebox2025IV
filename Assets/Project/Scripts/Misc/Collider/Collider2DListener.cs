using UnityEngine;

namespace Misc.Collider
{
    public class Collider2DListener : ObservableColliderBase<Collision2D>
    {
        private void OnCollisionEnter2D(Collision2D collision) => Enter(collision);
        private void OnCollisionExit2D(Collision2D collision) => Exit(collision);
    }
}