using Input;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public interface IUpdatable
    {
        void Update();
    }

    public interface IFixedUpdatable
    {
        void FixedUpdate();
    }

    public interface IExitable
    {
        void Exit();
    }

    public interface IMovementState : IUpdatable, IFixedUpdatable, IExitable
    {
    }

    public class GroundMovement : IMovementState
    {
        private const float SLOPE_GRAVITY_MULTIPLIER = 0;
        private const float DEFAULT_GRAVITY_MULTIPLIER = 1;

        private readonly UserInput userInput;
        private readonly PresetObject preset;
        private readonly Rigidbody2D body;
        private readonly GroundChecker groundChecker;

        private Vector2 desiredVelocity;
        private Vector2 velocity;
        private float inputX;
        private bool onGround;
        private bool onSlope;

        public GroundMovement(UserInput userInput, PresetObject preset, Rigidbody2D body, GroundChecker groundChecker)
        {
            this.userInput = userInput;
            this.preset = preset;
            this.body = body;
            this.groundChecker = groundChecker;
        }

        public void Update() => CheckMovement();

        public void FixedUpdate()
        {
            UpdateState();
            UpdateVelocity();
            HorizontalMovement();
            ApplyVelocity();
        }

        public void Exit()
        {
            // nothing here
        }

        private void UpdateState()
        {
            onGround = groundChecker.IsOnGround;
            onSlope = groundChecker.IsOnSlope(preset.GroundMovementSettings.maxSlopeAngle);
            body.gravityScale = GetGravity();
        }

        private void UpdateVelocity()
        {
            if (groundChecker.SlopeAngle > 0.1)
            {
                desiredVelocity = groundChecker.SlopePerpendicular * (groundChecker.SlopeAngle > preset.GroundMovementSettings.maxSlopeAngle ? inputX : -inputX);
            } else desiredVelocity = new Vector2(inputX, 0f);
            desiredVelocity *= Mathf.Max(preset.GroundMovementSettings.maxSpeed - preset.GroundMovementSettings.friction, 0f);
            velocity = body.linearVelocity - groundChecker.GetGroundVelocity();
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

        private void ApplyVelocity() => body.linearVelocity = velocity + groundChecker.GetGroundVelocity();

        private float GetGravity()
        {
            var multiplier = DEFAULT_GRAVITY_MULTIPLIER;

            if (onSlope) multiplier = SLOPE_GRAVITY_MULTIPLIER;
            else if (onGround && groundChecker.SlopeAngle > preset.GroundMovementSettings.maxSlopeAngle)
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