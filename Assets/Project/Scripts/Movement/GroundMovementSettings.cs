using System;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class GroundMovementSettings
    {
        [Tooltip("Friction to apply against movement")]
        public float friction;

        [Range(0f, 20f)] [Tooltip("Maximum movement speed")]
        public float maxSpeed;

        [Range(0f, 100f)] [Tooltip("How fast to stop when changing direction")]
        public float maxTurnSpeed;

        [Range(0f, 100f)] [Tooltip("How fast to reach max speed")]
        public float maxAcceleration;

        [Range(0f, 100f)] [Tooltip("How fast to stop after letting go")]
        public float maxDeceleration;

        [Tooltip("When false, the character will skip acceleration and deceleration and instantly move and stop")]
        public bool useAcceleration;

        [Range(0, 89)] public float maxSlopeAngle;
    }
}