using Input;
using Reflex.Attributes;
using UnityEngine;

namespace Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        
        [SerializeField] [Tooltip("Friction to apply against movement on stick")]
        private float friction;

        [Header("Options")]
        [Tooltip("When false, the character will skip acceleration and deceleration and instantly move and stop")]
        public bool useAcceleration;
        
        private float maxSpeed = 10f;
        private float maxAcceleration = 52f;
        private float maxDeceleration = 52f;
        private float maxTurnSpeed = 80f;
        private float maxAirAcceleration;
        private float maxAirDeceleration;
        private float maxAirTurnSpeed = 80f;
        
        private Rigidbody2D body;
        private CharacterGround ground;
        [Inject] private UserInput userInput;

        private float directionX;
        private Vector2 desiredVelocity;
        private Vector2 velocity;
        private float maxSpeedChange;
        private float acceleration;
        private float deceleration;
        private float turnSpeed;

        private bool onGround;
        private bool pressingKey;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            ground = GetComponent<CharacterGround>();
        }

        private void Update()
        {
            CheckMovement();
            if (directionX != 0)
            {
                transform.localScale = new Vector3(directionX > 0 ? 1 : -1, 1, 1);
                pressingKey = true;
            }
            else pressingKey = false;

            desiredVelocity = new Vector2(directionX, 0f) * Mathf.Max(maxSpeed - friction, 0f);
        }

        private void FixedUpdate()
        {
            onGround = ground.GetOnGround();

            velocity = body.linearVelocity;

            if (useAcceleration) MoveWithAcceleration();
            else
            {
                if (onGround) MoveWithoutAcceleration();
                else MoveWithAcceleration();
            }
        }

        private void CheckMovement() => directionX = userInput.Enabled ? userInput.Movement.x : 0;

        private void MoveWithAcceleration()
        {
            acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            deceleration = onGround ? maxDeceleration : maxAirDeceleration;
            turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

            if (pressingKey)
            {
                if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x)) maxSpeedChange = turnSpeed * Time.deltaTime;
                else maxSpeedChange = acceleration * Time.deltaTime;
            }
            else maxSpeedChange = deceleration * Time.deltaTime;

            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            body.linearVelocity = velocity;
        }

        private void MoveWithoutAcceleration()
        {
            velocity.x = desiredVelocity.x;
            body.linearVelocity = velocity;
        }

        public void InstallPreset(PresetObject preset)
        {
            maxAcceleration = preset.Acceleration;
            maxSpeed = preset.TopSpeed;
            maxDeceleration = preset.Deceleration;
            maxTurnSpeed = preset.TurnSpeed;
            maxAirAcceleration = preset.AirControl;
            maxAirDeceleration = preset.AirBrake;
            maxAirTurnSpeed = preset.AirControlActual;
        }
    }
}