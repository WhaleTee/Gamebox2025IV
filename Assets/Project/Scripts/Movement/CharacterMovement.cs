using Input;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        private const float SLOPE_GRAVITY_MULTIPLIER = 0;
        private const float DEFAULT_GRAVITY_MULTIPLIER = 1;
        [SerializeField] private PresetObject preset;
        [Inject] private UserInput userInput;
        private Rigidbody2D body;
        private GroundChecker groundChecker;

        // ground movement
        private Vector2 desiredVelocity;
        private Vector2 velocity;
        private float inputX;
        private float speed;
        private float acceleration;
        private float deceleration;
        private float turnSpeed;
        private bool onGround;
        private bool onSlope;

        // air movement
        private bool desiredJump;
        private bool pressingJump;
        private bool currentJump;
        private bool canJumpAgain;
        private float jumpBufferCounter;
        private float coyoteTimeCounter;
        private int jumpCount;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.interpolation = RigidbodyInterpolation2D.Extrapolate;
            groundChecker = GetComponent<GroundChecker>();
        }

        private void Start()
        {
            userInput.SubscribeJumpPerformed(OnJumpPerformed);
            userInput.SubscribeJumpCanceled(OnJumpCanceled);
        }

        private void Update()
        {
            onGround = groundChecker.IsOnGround;
            onSlope = groundChecker.IsOnSlope(preset.GroundMovementSettings.maxSlopeAngle);
            CheckMovement();
            UpdateVelocity();
            UpdateJumpBuffer();
            UpdateCoyoteTime();
            UpdateJumpState();
            if (inputX != 0) transform.localScale = new Vector3(inputX > 0 ? 1 : -1, 1, 1);
        }

        private void FixedUpdate()
        {
            UpdateState();
            UpdateMovement();
            ApplyVelocity();
        }

        private void OnDestroy()
        {
            userInput.UnsubscribeJumpPerformed(OnJumpPerformed);
            userInput.UnsubscribeJumpCanceled(OnJumpCanceled);
        }

        private void UpdateState()
        {
            onGround = groundChecker.IsOnGround;
            onSlope = groundChecker.IsOnSlope(preset.GroundMovementSettings.maxSlopeAngle);
            body.gravityScale = GetGravity();
            velocity = body.linearVelocity;
        }

        private void UpdateVelocity()
        {
            var maxSpeed = onGround
                ? preset.GroundMovementSettings.maxSpeed
                : preset.AirMovementSettings.maxSpeed;
            var friction = onGround ? preset.GroundMovementSettings.friction : 0;
            var groundVelocity = groundChecker.GetGroundVelocity();
            desiredVelocity = onSlope ? groundChecker.SlopePerpendicular * -inputX : new Vector2(inputX, 0f);
            desiredVelocity *= Mathf.Max(maxSpeed - friction, 0f);
            if (desiredVelocity.magnitude < groundVelocity.magnitude) desiredVelocity = groundVelocity;
        }

        private void UpdateJumpBuffer()
        {
            if (preset.AirMovementSettings.jumpBuffer > 0 && currentJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > preset.AirMovementSettings.jumpBuffer)
                {
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            } else jumpBufferCounter = 0;
        }

        private void UpdateCoyoteTime()
        {
            if (!currentJump && !onGround) coyoteTimeCounter += Time.deltaTime;
            else coyoteTimeCounter = 0;
        }

        private void UpdateJumpState()
        {
            if (!onGround) return;
            jumpCount = 0;
            currentJump = false;
        }

        private void UpdateMovement()
        {
            HorizontalMovement();
            VerticalMovement();
        }

        private void HorizontalMovement()
        {
            if (preset.GroundMovementSettings.useAcceleration || !onGround) MoveWithAcceleration();
            else MoveWithoutAcceleration();
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
            acceleration = onGround
                ? preset.GroundMovementSettings.maxAcceleration
                : preset.AirMovementSettings.maxAcceleration;
            deceleration = onGround
                ? preset.GroundMovementSettings.maxDeceleration
                : preset.AirMovementSettings.maxDeceleration;
            turnSpeed = onGround
                ? preset.GroundMovementSettings.maxTurnSpeed
                : preset.AirMovementSettings.maxTurnSpeed;

            if (inputX != 0) speed = Mathf.Sign(inputX) != Mathf.Sign(velocity.x) ? turnSpeed : acceleration;
            else speed = deceleration;

            speed *= Time.deltaTime;

            if (onSlope) velocity = Vector2.MoveTowards(velocity, desiredVelocity, speed);
            else velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, speed);
        }
        
        private void MoveWithoutAcceleration()
        {
            if (onSlope) velocity = desiredVelocity;
            else velocity.x = desiredVelocity.x;
        }
        
        private void Jump()
        {
            if (onGround || (coyoteTimeCounter > 0 && coyoteTimeCounter < preset.AirMovementSettings.coyoteTime) || canJumpAgain)
            {
                desiredJump = false;
                currentJump = true;
                jumpBufferCounter = 0;
                coyoteTimeCounter = 0;
                canJumpAgain = jumpCount++ <= preset.AirMovementSettings.maxAirJumps && !canJumpAgain;
                ApplyJumpInitialVelocity();
            }

            if (preset.AirMovementSettings.jumpBuffer == 0) desiredJump = false;
        }

        private void ApplyJumpInitialVelocity()
        {
            var jumpVelocity = GetJumpInitialVelocity();
            if (!onGround && velocity.y > 0f) jumpVelocity = Mathf.Max(jumpVelocity - velocity.y, 0f);
            else if (!onGround && velocity.y < 0f) jumpVelocity += Mathf.Abs(body.linearVelocity.y);

            velocity.y += jumpVelocity;
        }

        private void ApplyVelocity() => body.linearVelocity = velocity;

        private float GetGravity()
        {
            float multiplier;

            if (
                preset.AirMovementSettings.variableJumpHeight
                && velocity.y > 0
                && !onGround
                && (!pressingJump || !currentJump)
            ) multiplier = preset.AirMovementSettings.jumpCutOff;
            else if (onSlope && !desiredJump) multiplier = SLOPE_GRAVITY_MULTIPLIER;
            else if (onGround && !desiredJump) multiplier = DEFAULT_GRAVITY_MULTIPLIER;
            else multiplier = velocity.y < 0 && !desiredJump ? preset.AirMovementSettings.downwardGravityMultiplier : preset.AirMovementSettings.upwardGravityMultiplier;
            return GetJumpGravity() / Physics2D.gravity.y * multiplier;
        }

        private float GetJumpGravity()
        {
            return -2 * preset.AirMovementSettings.jumpHeight / Mathf.Pow(preset.AirMovementSettings.timeToJumpApex, 2);
        }

        private float GetJumpInitialVelocity() => Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * preset.AirMovementSettings.jumpHeight);

        private void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
            desiredJump = true;
            pressingJump = true;
        }

        private void OnJumpCanceled(InputAction.CallbackContext ctx) => pressingJump = false;
    }
}