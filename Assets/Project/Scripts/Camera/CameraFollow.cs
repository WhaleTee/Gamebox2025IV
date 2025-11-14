using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float speed;
        private GameObject target;

        private void LateUpdate()
        {
            if (target == null) target = GameObject.FindWithTag("Player");
            if (target == null) return;
            var targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}