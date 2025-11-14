using Input;
using UnityEngine;

namespace Movement
{
    public class GroundMovement : IMovementState
    {
        private const float SLOPE_GRAVITY_MULTIPLIER = 0;
        private const float DEFAULT_GRAVITY_MULTIPLIER = 1;

        private readonly UserInput userInput;
        private readonly PresetObject preset;
        private readonly Rigidbody2D body;
        private readonly EnvironmentSensor environmentSensor;

        private Vector2 desiredVelocity;
        private Vector2 velocity;
        private float inputX;
        private bool onGround;
        private bool onSlope;

        public GroundMovement(UserInput userInput, PresetObject preset, Rigidbody2D body, EnvironmentSensor environmentSensor)
        {
            this.userInput = userInput;
            this.preset = preset;
            this.body = body;
            this.environmentSensor = environmentSensor;
        }

        public override void Update() => CheckMovement();

        public override void FixedUpdate()
        {
            UpdateState();
            UpdateVelocity();
            HorizontalMovement();
            ApplyVelocity();
        }

        private void UpdateState()
        {
            onGround = environmentSensor.IsOnGround;
            onSlope = environmentSensor.IsOnSlope(preset.GroundMovementSettings.maxSlopeAngle);
            body.gravityScale = GetGravity();
        }

        private void UpdateVelocity()
        {
            if (environmentSensor.SlopeAngle > 0.1)
            {
                desiredVelocity = environmentSensor.SlopePerpendicular * (environmentSensor.SlopeAngle > preset.GroundMovementSettings.maxSlopeAngle ? inputX : -inputX);
            } else desiredVelocity = new Vector2(inputX, 0f);
            desiredVelocity *= Mathf.Max(preset.GroundMovementSettings.maxSpeed - preset.GroundMovementSettings.friction, 0f);
            velocity = body.linearVelocity - environmentSensor.GetGroundVelocity();
        }

        private void HorizontalMovement()
        {
            if (preset.GroundMovementSettings.useAcceleration) MoveWithAcceleration();
            else MoveWithoutAcceleration();
        }

        private void CheckMovement() => inputX = userInput.Enabled ? userInput.Movement.x : 0;

        private void MoveWithAcceleration()
        {
            float speed;
            if (inputX != 0)
                speed = Mathf.Sign(inputX) != Mathf.Sign(velocity.x)
                    ? preset.GroundMovementSettings.maxTurnSpeed
                    : preset.GroundMovementSettings.maxAcceleration;
            else speed = preset.GroundMovementSettings.maxDeceleration;

            speed *= Time.deltaTime;

            if (onSlope) velocity = Vector2.MoveTowards(velocity, desiredVelocity, speed);
            else velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, speed);
        }

        private void MoveWithoutAcceleration()
        {
            if (onSlope) velocity = desiredVelocity;
            else velocity.x = desiredVelocity.x;
        }

        private void ApplyVelocity() => body.linearVelocity = velocity + environmentSensor.GetGroundVelocity();

        private float GetGravity()
        {
            var multiplier = DEFAULT_GRAVITY_MULTIPLIER;

            if (onSlope) multiplier = SLOPE_GRAVITY_MULTIPLIER;
            else if (onGround && environmentSensor.SlopeAngle > preset.GroundMovementSettings.maxSlopeAngle)
            {
                multiplier = preset.AirMovementSettings.downwardGravityMultiplier;
            }

            return GetJumpGravity() / Physics2D.gravity.y * multiplier;
        }

        private float GetJumpGravity()
        {
            return -2 * preset.AirMovementSettings.jumpHeight / Mathf.Pow(preset.AirMovementSettings.timeToJumpApex, 2);
        }
    }
}