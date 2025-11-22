using Environment;
using Input;
using UnityEngine;

namespace Movement
{
    public class StairsMovement : MovementState
    {
        private const float STAIRS_GRAVITY_MULTIPLIER = 0;
        
        private readonly UserInput userInput;
        private readonly PresetObject preset;
        private readonly Rigidbody2D body;
        private readonly EnvironmentSensor environmentSensor;
        private GameObject stairs;
        private Vector2 desiredVelocity;
        private Vector2 velocity;
        private Vector2 input;

        public StairsMovement(UserInput userInput, PresetObject preset, Rigidbody2D body, EnvironmentSensor environmentSensor)
        {
            this.userInput = userInput;
            this.preset = preset;
            this.body = body;
            this.environmentSensor = environmentSensor;
        }

        public override void Update()
        {
            input = userInput.Movement;
        }
        
        public override void FixedUpdate()
        {
            UpdateState();
            UpdateVelocity();
            HorizontalMovement();
            ApplyVelocity();
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (stairs) body.position = new Vector2(stairs.transform.position.x, body.position.y);
        }

        private void UpdateState()
        {
            stairs = environmentSensor.CheckForStairs();
            body.gravityScale = GetGravity();
        }

        private void UpdateVelocity()
        {
            desiredVelocity.y = input.y;
            desiredVelocity *= Mathf.Max(preset.StairsMovementSettings.maxSpeed - preset.StairsMovementSettings.friction, 0f);
            velocity = body.linearVelocity;
        }
        
        private void HorizontalMovement()
        {
            if (preset.StairsMovementSettings.useAcceleration) MoveWithAcceleration();
            else MoveWithoutAcceleration();
        }
        
        private void MoveWithAcceleration()
        {
            float speed;
            if (input != Vector2.zero)
                speed = Mathf.Sign(input.y) != Mathf.Sign(velocity.y)
                    ? preset.StairsMovementSettings.maxTurnSpeed
                    : preset.StairsMovementSettings.maxAcceleration;
            else speed = preset.StairsMovementSettings.maxDeceleration;

            speed *= Time.deltaTime;

            velocity = Vector2.MoveTowards(velocity, desiredVelocity, speed);
        }

        private void MoveWithoutAcceleration() => velocity = desiredVelocity;

        private void ApplyVelocity() => body.linearVelocity = velocity + environmentSensor.GetGroundVelocity();
        
        private float GetGravity() => STAIRS_GRAVITY_MULTIPLIER;
    }
}