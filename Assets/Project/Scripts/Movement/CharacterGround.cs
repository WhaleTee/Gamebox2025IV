using UnityEngine;


namespace Project.Scripts.Movement
{
    public class CharacterGround : MonoBehaviour
    {
        private bool onGround;

        [Header("Collider Settings")]
        [SerializeField]
        [Tooltip("Length of the ground-checking collider")]
        private float groundLength = 0.95f;

        [SerializeField] [Tooltip("Distance between the ground-checking colliders")]
        private Vector3 colliderOffset;

        [Header("Layer Masks")] [SerializeField] [Tooltip("Which layers are read as the ground")]
        private LayerMask groundLayer;


        private void Update()
        {
            onGround =
                Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer)
                || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        }

        public bool GetOnGround()
        {
            return onGround;
        }
    }
}