using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Movement
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] [Tooltip("Length of the ground-checking ray")]
        private float groundLength = 0.95f;

        [SerializeField] [Tooltip("Distance between the ground-checking rays")]
        private Vector3 colliderOffset;

        [SerializeField] [Tooltip("Which layers are read as the ground")]
        private LayerMask groundLayer;

        public bool IsOnGround { get; private set; }
        public float SlopeAngle { get; private set; }
        public Vector2 SlopePerpendicular { get; private set; }
        public Vector2 SlopeNormal { get; private set; }

        private readonly List<GameObject> groundObjects = new List<GameObject>(2);

        private void Update()
        {
            groundObjects.Clear();
            var rayOnePosition = transform.position + colliderOffset;
            var rayTwoPosition = transform.position - colliderOffset;
            var rayOne = Physics2D.Raycast(rayOnePosition, Vector2.down, groundLength, groundLayer);
            var rayTwo = Physics2D.Raycast(rayTwoPosition, Vector2.down, groundLength, groundLayer);
            IsOnGround = rayOne || rayTwo;
            groundObjects.Add(rayOne ? rayOne.collider.gameObject : null);
            groundObjects.Add(rayTwo ? rayTwo.collider.gameObject : null);
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
                transform.position + Vector3.down + (Vector3)SlopePerpendicular * groundLength);
        }

        public bool IsOnSlope(float angle) => IsOnGround && SlopeAngle > 0.1f && SlopeAngle < angle;

        public Vector2 GetGroundVelocity()
        {
            var result = Vector2.zero;
            if (groundObjects.Count < 1) return result;
            if (groundObjects[0] != null && groundObjects[0].TryGetComponent(out MovingPlatform pOne))
            {
                result = pOne.Velocity;
            }
            else if (groundObjects[1] != null && groundObjects[1].TryGetComponent(out MovingPlatform pTwo))
            {
                result = pTwo.Velocity;
            }
            return result;
        }
    }
}