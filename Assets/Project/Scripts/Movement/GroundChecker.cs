using UnityEngine;


namespace Movement
{
    public class GroundChecker : MonoBehaviour
    {
        public bool IsOnGround { get; private set; }
        public float SlopeAngle { get; private set; }
        public Vector2 SlopePerpendicular { get; private set; }
        public Vector2 SlopeNormal { get; private set; }

        [SerializeField] [Tooltip("Length of the ground-checking ray")]
        private float groundLength = 0.95f;

        [SerializeField] [Tooltip("Distance between the ground-checking rays")]
        private Vector3 colliderOffset;

        [SerializeField] [Tooltip("Which layers are read as the ground")]
        private LayerMask groundLayer;
        private void Update()
        {
            var rayOnePosition = transform.position + colliderOffset;
            var rayTwoPosition = transform.position - colliderOffset;
            var rayOne = Physics2D.Raycast(rayOnePosition, Vector2.down, groundLength, groundLayer);
            var rayTwo = Physics2D.Raycast(rayTwoPosition, Vector2.down, groundLength, groundLayer);
            IsOnGround = rayOne || rayTwo;
            if (!IsOnGround)
            {
                SlopeAngle = 0;
                SlopeNormal = Vector2.up;
                SlopePerpendicular = Vector2.left;
                return;
            }
            var angleOne = Vector2.Angle(rayOne.normal, Vector2.up);
            var angleTwo = Vector2.Angle(rayTwo.normal, Vector2.up);
            SlopeAngle = Mathf.Max(angleOne, angleTwo);
            if (SlopeAngle == 0) return;

            if (angleOne > angleTwo)
            {
                SlopeNormal = rayOne.normal;
                SlopePerpendicular = Vector2.Perpendicular(rayOne.normal).normalized;
            }
            else
            {
                SlopeNormal = rayTwo.normal;
                SlopePerpendicular = Vector2.Perpendicular(rayTwo.normal).normalized;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsOnGround ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position + colliderOffset,
                transform.position + colliderOffset + Vector3.down * groundLength);
            Gizmos.DrawLine(transform.position - colliderOffset,
                transform.position - colliderOffset + Vector3.down * groundLength);
            Gizmos.DrawLine(transform.position + Vector3.down,
                transform.position + Vector3.down + (Vector3) SlopePerpendicular * groundLength);
        }
        
        public bool IsOnSlope(float angle) => IsOnGround && SlopeAngle > 0.1f && SlopeAngle < angle;

    }
}