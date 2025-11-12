using System;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class AirMovementSettings
    {
        [Range(0f, 20f)] [Tooltip("Maximum movement speed")]
        public float maxSpeed;

        [Range(0f, 100f)] [Tooltip("How fast to stop in mid-air when no direction is used")]
        public float maxTurnSpeed;

        [Range(0f, 100f)] [Tooltip("How fast to reach max speed when in mid-air")]
        public float maxAcceleration;

        [Range(0f, 100f)] [Tooltip("How fast to stop when changing direction when in mid-air")]
        public float maxDeceleration;

        [Range(0f, 5f)] [Tooltip("Gravity multiplier to apply when going up")]
        public float upwardGravityMultiplier = 1f;

        [Range(1f, 10f)] [Tooltip("Gravity multiplier to apply when coming down")]
        public float downwardGravityMultiplier;

        [SerializeField] [Tooltip("The fastest speed the character can fall and jump")]
        public float speedLimit;

        [Range(0f, 0.3f)] [Tooltip("How long should coyote time last?")]
        public float coyoteTime;

        [Range(0f, 0.3f)] [Tooltip("How far from ground should we cache your jump?")]
        public float jumpBuffer;

        [Range(2f, 5.5f)] [Tooltip("Maximum jump height")]
        public float jumpHeight;

        [Range(0.2f, 1.25f)] [Tooltip("How long it takes to reach that height before coming back down")]
        public float timeToJumpApex;

        [Range(1f, 10f)] [Tooltip("Gravity multiplier when you let go of jump")]
        public float jumpCutOff;

        [Range(0, 1)] [Tooltip("How many times can you jump in the air?")]
        public int maxAirJumps;

        [Tooltip("Should the character drop when you let go of jump?")]
        public bool variableJumpHeight;
    }
}