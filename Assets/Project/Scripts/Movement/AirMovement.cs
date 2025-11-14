using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class AirMovement : IMovementState, IDisposable
    {
        private readonly UserInput userInput;
        private readonly PresetObject preset;
        private readonly Rigidbody2D body;
        private readonly EnvironmentSensor environmentSensor;

        // ground movement
        private Vector2 desiredVelocity;
        private Vector2 velocity;
        private float inputX;

        // air movement
        private bool desiredJump;
        private bool pressingJump;
        private bool currentJump;
        private bool canJumpAgain;
        private float jumpBufferCounter;
        private float coyoteTimeCounter;
        private int jumpCount;

        public AirMovement(UserInput userInput, PresetObject preset, Rigidbody2D body, EnvironmentSensor environmentSensor)
        {
            this.userInput = userInput;
            this.preset = preset;
            this.body = body;
            this.environmentSensor = environmentSensor;
            userInput.SubscribeJumpPerformed(OnJumpPerformed);
            userInput.SubscribeJumpCanceled(OnJumpCanceled);
        }

        public override void Update()
        {
            CheckMovement();
            UpdateJumpBuffer();
            UpdateCoyoteTime();
        }

        public override void FixedUpdate()
        {
            UpdateState();
            UpdateVelocity();
            UpdateMovement();
            ApplyVelocity();
        }

        public override void Exit() => ResetJumpState();

        public void Dispose()
        {
            userInput.UnsubscribeJumpPerformed(OnJumpPerformed);
            userInput.UnsubscribeJumpCanceled(OnJumpCanceled);
        }

        public bool IsJumpBofferActive() => jumpBufferCounter > 0;

        private void UpdateState()
        {
            body.gravityScale = GetGravity();
            velocity = body.linearVelocity;
        }

        private void UpdateVelocity() => desiredVelocity = new Vector2(inputX * preset.AirMovementSettings.maxSpeed, 0f);

        private void UpdateJumpBuffer()
        {
            if (preset.AirMovementSettings.jumpBuffer > 0 && desiredJump && !canJumpAgain)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > preset.AirMovementSettings.jumpBuffer)
                {
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
            else jumpBufferCounter = 0;
        }

        private void UpdateCoyoteTime()
        {
            if (!environmentSensor.IsOnGround && !currentJump) coyoteTimeCounter += Time.deltaTime;
            else coyoteTimeCounter = 0;
        }

        private void ResetJumpState()
        {
            jumpCount = 0;
            coyoteTimeCounter = 0;
            currentJump = false;
            canJumpAgain = false;
        }

        private void UpdateMovement()
        {
            HorizontalMovement();
            VerticalMovement();
        }

        private void HorizontalMovement()
        {
            MoveWithAcceleration();
        }

        private void VerticalMovement()
        {
            if (desiredJump) Jump();
            else
            {
                velocity.y = Mathf.Clamp(
                    velocity.y,
                    -preset.AirMovementSettings.speedLimit,
                    preset.AirMovementSettings.speedLimit
                );
            }
        }

        private void CheckMovement() => inputX = userInput.Enabled ? userInput.Movement.x : 0;

        private void MoveWithAcceleration()
        {
            float speed;
            if (inputX != 0)
                speed = Mathf.Sign(inputX) != Mathf.Sign(velocity.x)
                    ? preset.AirMovementSettings.maxTurnSpeed
                    : preset.AirMovementSettings.maxAcceleration;
            else speed = preset.AirMovementSettings.maxDeceleration;

            speed *= Time.deltaTime;

            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, speed);
        }

        private void Jump()
        {
            if (environmentSensor.IsOnGround || (coyoteTimeCounter > 0 && coyoteTimeCounter < preset.AirMovementSettings.coyoteTime) || canJumpAgain)
            {
                desiredJump = false;
                currentJump = true;
                jumpBufferCounter = 0;
                coyoteTimeCounter = 0;
                canJumpAgain = ++jumpCount <= preset.AirMovementSettings.maxAirJumps && !canJumpAgain;
                ApplyJumpInitialVelocity();
            }

            if (preset.AirMovementSettings.jumpBuffer == 0) desiredJump = false;
        }

        private void ApplyJumpInitialVelocity()
        {
            var jumpVelocity = GetJumpInitialVelocity();
            if (velocity.y > 0f)
                jumpVelocity = Mathf.Max(jumpVelocity - velocity.y + environmentSensor.GetGroundVelocity().y, 0f);
            else if (velocity.y < 0f) jumpVelocity += Mathf.Abs(body.linearVelocity.y);

            velocity.y += jumpVelocity;
        }

        private void ApplyVelocity() => body.linearVelocity = velocity;

        private float GetGravity()
        {
            float multiplier;

            if (preset.AirMovementSettings.variableJumpHeight && velocity.y > 0 && (!pressingJump || !currentJump))
            {
                multiplier = preset.AirMovementSettings.jumpCutOff;
            }
            else
            {
                multiplier = velocity.y < 0 && !desiredJump
                    ? preset.AirMovementSettings.downwardGravityMultiplier
                    : preset.AirMovementSettings.upwardGravityMultiplier;
            }

            return GetJumpGravity() / Physics2D.gravity.y * multiplier;
        }

        private float GetJumpGravity()
        {
            return -2 * preset.AirMovementSettings.jumpHeight / Mathf.Pow(preset.AirMovementSettings.timeToJumpApex, 2);
        }

        private float GetJumpInitialVelocity() =>
            Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * preset.AirMovementSettings.jumpHeight);

        private void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
            desiredJump = true;
            pressingJump = true;
        }

        private void OnJumpCanceled(InputAction.CallbackContext ctx) => pressingJump = false;
    }
}