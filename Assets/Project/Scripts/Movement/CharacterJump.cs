using Project.Scripts.Input;
using UnityEngine;

namespace Project.Scripts.Movement
{
    public class CharacterJump : MonoBehaviour
    {
        [SerializeField, Range(0f, 5f)] [Tooltip("Gravity multiplier to apply when going up")]
        private float upwardMovementMultiplier = 1f;
        
        [SerializeField] [Tooltip("The fastest speed the character can fall")]
        private float speedLimit;

        [SerializeField, Range(0f, 0.3f)] [Tooltip("How long should coyote time last?")]
        private float coyoteTime = 0.15f;

        [SerializeField, Range(0f, 0.3f)] [Tooltip("How far from ground should we cache your jump?")]
        private float jumpBuffer = 0.15f;

        private float jumpHeight = 7.3f;
        private float timeToJumpApex;
        private float downwardMovementMultiplier = 6.17f;
        private float jumpCutOff;
        private int maxAirJumps;
        private bool variableJumpHeight;
        
        private float jumpSpeed;
        private float defaultGravityScale;
        private float gravMultiplier;
        private bool canJumpAgain;

        private Rigidbody2D body;
        private CharacterGround ground;
        private Vector2 velocity;

        private bool desiredJump;
        private float jumpBufferCounter;
        private float coyoteTimeCounter;
        private bool pressingJump;
        private bool onGround;
        private bool currentlyJumping;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            ground = GetComponent<CharacterGround>();
            defaultGravityScale = 1f;
        }

        private void Update()
        {
            CheckJump();
            ApplyPhysics();

            onGround = ground.GetOnGround();

            if (jumpBuffer > 0)
            {
                if (desiredJump)
                {
                    jumpBufferCounter += Time.deltaTime;

                    if (jumpBufferCounter > jumpBuffer)
                    {
                        desiredJump = false;
                        jumpBufferCounter = 0;
                    }
                }
            }

            if (!currentlyJumping && !onGround) coyoteTimeCounter += Time.deltaTime;
            else coyoteTimeCounter = 0;
        }

        private void FixedUpdate()
        {
            velocity = body.linearVelocity;

            if (desiredJump)
            {
                DoJump();
                body.linearVelocity = velocity;
                return;
            }

            CalculateGravity();
        }

        private void CheckJump()
        {
            desiredJump = UserInput.Instance.JumpPerformed;
            pressingJump = UserInput.Instance.JumpPressed;
        }

        private void ApplyPhysics()
        {
            var newGravity = new Vector2(0, -2 * jumpHeight / (timeToJumpApex * timeToJumpApex));
            body.gravityScale = newGravity.y / Physics2D.gravity.y * gravMultiplier;
        }

        private void CalculateGravity()
        {
            if (body.linearVelocity.y > 0.01f)
            {
                if (onGround) gravMultiplier = defaultGravityScale;
                else
                {
                    if (variableJumpHeight)
                    {
                        if (pressingJump && currentlyJumping) gravMultiplier = upwardMovementMultiplier;
                        else gravMultiplier = jumpCutOff;
                    }
                    else gravMultiplier = upwardMovementMultiplier;
                }
            }
            else if (body.linearVelocity.y < -0.01f)
            {
                gravMultiplier = onGround ? defaultGravityScale : downwardMovementMultiplier;
            }
            else
            {
                if (onGround) currentlyJumping = false;
                gravMultiplier = defaultGravityScale;
            }

            body.linearVelocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, speedLimit));
        }

        private void DoJump()
        {
            if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime) || canJumpAgain)
            {
                desiredJump = false;
                jumpBufferCounter = 0;
                coyoteTimeCounter = 0;

                canJumpAgain = maxAirJumps == 1 && !canJumpAgain;

                jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * jumpHeight);

                if (velocity.y > 0f) jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
                else if (velocity.y < 0f) jumpSpeed += Mathf.Abs(body.linearVelocity.y);

                velocity.y += jumpSpeed;
                currentlyJumping = true;
            }

            if (jumpBuffer == 0) desiredJump = false;
        }

        public void InstallPreset(PresetObject preset)
        {
            jumpHeight = preset.JumpHeight;
            timeToJumpApex = preset.TimeToApex;
            downwardMovementMultiplier = preset.DownwardMovementMultiplier;
            jumpCutOff = preset.JumpCutoff;
            maxAirJumps = preset.AirJumps;
            variableJumpHeight = preset.VariableJumpHeight;
        }
    }
}