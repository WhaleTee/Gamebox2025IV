using System;
using Input;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private PresetObject preset;
        [Inject] private UserInput userInput;
        private Rigidbody2D body;
        private EnvironmentSensor environmentSensor;

        private GroundMovement groundMovement;
        private AirMovement airMovement;
        private StairsMovement stairsMovement;
        private MovementState currentState;
        private bool desiredJump;

        public event Action<MovementState> StateChange;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            environmentSensor = GetComponent<EnvironmentSensor>();
        }

        private void Start()
        {
            groundMovement = new GroundMovement(userInput, preset, body, environmentSensor);
            airMovement = new AirMovement(userInput, preset, body, environmentSensor);
            stairsMovement = new StairsMovement(userInput, preset, body, environmentSensor);
            ChangeState(groundMovement);
            userInput.SubscribeJumpPerformed(OnJumpPerformed);
        }

        private void Update()
        {
            if (userInput.Movement.x != 0) transform.localScale = new Vector3(userInput.Movement.x > 0 ? 1 : -1, 1, 1);
            groundMovement.Update();
            airMovement.Update();
            stairsMovement.Update();
        }

        private void FixedUpdate()
        {
            CheckForStateTransition();
            currentState.FixedUpdate();
            desiredJump = false;
        }

        private void OnDestroy()
        {
            userInput.UnsubscribeJumpPerformed(OnJumpPerformed);
            airMovement.Dispose();
        }

        private void CheckForStateTransition()
        {
            var stairs = environmentSensor.CheckForStairs();
            if (stairs && userInput.Movement != Vector2.zero)
            {
                if (Vector2.Dot(stairs.transform.up, userInput.Movement) > 0 || Vector2.Dot(stairs.transform.right, userInput.Movement) > 0)
                {
                    ChangeState(stairsMovement);
                }
            }
            else if (currentState is StairsMovement)
            {
                if (environmentSensor.IsOnGround && userInput.Movement == Vector2.zero) ChangeState(groundMovement);
                if (stairs == null && !environmentSensor.IsOnGround) ChangeState(groundMovement);
            }
            else if (
                (!environmentSensor.IsOnGround ||
                 (environmentSensor.SlopeAngle < preset.GroundMovementSettings.maxSlopeAngle && desiredJump) ||
                 (environmentSensor.SlopeAngle == 0 && body.linearVelocityY != 0)) && currentState is GroundMovement)
            {
                ChangeState(airMovement);
            }
            else if (environmentSensor.IsOnGround && currentState is AirMovement && !airMovement.IsJumpBofferActive())
            {
                ChangeState(groundMovement);
            }
        }

        private void ChangeState(MovementState state)
        {
            currentState?.Exit();
            currentState = state;
            StateChange?.Invoke(currentState);
        }

        private void OnJumpPerformed(InputAction.CallbackContext ctx) => desiredJump = true;
    }
}