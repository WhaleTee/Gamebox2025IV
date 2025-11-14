using System;
using System.Collections.Generic;
using UnityEngine;
using Environment;


namespace Movement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EnvironmentSensor : MonoBehaviour
    {
        [SerializeField] [Tooltip("Length of the ground-checking ray")]
        private float groundLength;

        [SerializeField] [Tooltip("Distance between the ground-checking rays")]
        private Vector3 colliderOffset;

        [SerializeField] [Tooltip("Which layers are read as the ground")]
        private LayerMask groundLayer;

        [SerializeField] [Tooltip("Which layers are read as the stairs")]
        private LayerMask stairsLayer;
        
        
        private readonly List<GameObject> groundObjects = new List<GameObject>(2);
        private BoxCollider2D sensorCollider;

        public bool IsOnGround { get; private set; }
        public float SlopeAngle { get; private set; }
        public Vector2 SlopePerpendicular { get; private set; }
        public Vector2 SlopeNormal { get; private set; }


        private void Awake()
        {
            sensorCollider = GetComponent<BoxCollider2D>();
        }

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

        public GameObject CheckForStairs()
        {
            var results = new Collider2D[1];
            var filter = new ContactFilter2D();
            filter.SetLayerMask(stairsLayer);
            Physics2D.OverlapBox(transform.position - (Vector3)sensorCollider.offset, sensorCollider.size, 0, filter, results);
            return results[0] != null ? results[0].gameObject : null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsOnGround ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
            Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
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