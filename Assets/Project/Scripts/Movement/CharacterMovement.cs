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
        private Collider2D mainCollider;
        private EnvironmentSensor environmentSensor;

        private GroundMovement groundMovement;
        private AirMovement airMovement;
        private StairsMovement stairsMovement;
        private MovementState currentState;
        private bool desiredJump;

        public Vector2 GroundVelocity { get; private set; }
        public Vector2 Direction { get; private set; }

        public event Action<MovementState> StateChange;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            mainCollider = GetComponent<Collider2D>();
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
            if (userInput.Movement.x != 0)
            {
                Direction = userInput.Movement;
                if (Direction.y == 0) Direction = new Vector2(Direction.x, 1);
                transform.localScale = new Vector3(Direction.x > 0 ? 1 : -1, 1, 1);
            }
            groundMovement.Update();
            airMovement.Update();
            stairsMovement.Update();
        }

        private void FixedUpdate()
        {
            GroundVelocity = environmentSensor.GetGroundVelocity();
            CheckForStateTransition();
            currentState.FixedUpdate();
            desiredJump = false;
            mainCollider.enabled = currentState is not StairsMovement;
        }

        private void OnDestroy()
        {
            userInput.UnsubscribeJumpPerformed(OnJumpPerformed);
            airMovement.Dispose();
        }

        private void CheckForStateTransition()
        {
            var stairs = environmentSensor.CheckForStairs();
            if (currentState is not StairsMovement && stairs && userInput.Movement.y != 0)
            {
                if (userInput.Movement.y > 0 && environmentSensor.IsStairsOver)
                {
                    transform.position += Vector3.up;
                    ChangeState(stairsMovement);
                }
                else if (userInput.Movement.y < 0 && environmentSensor.IsStairsUnder)
                {
                    transform.position += Vector3.down;
                    ChangeState(stairsMovement);
                }
            }
            else if (currentState is StairsMovement)
            {
                if (environmentSensor.IsOnGround && desiredJump) ChangeState(airMovement);
                else if (environmentSensor.IsOnGround && (userInput.Movement.x != 0 || stairs == null)) ChangeState(groundMovement);
                else if (!environmentSensor.IsOnGround && stairs == null) ChangeState(airMovement);
            }
            else if (
                (!environmentSensor.IsOnGround ||
                 (environmentSensor.SlopeAngle < preset.GroundMovementSettings.maxSlopeAngle && desiredJump) ||
                 (!environmentSensor.IsOnGround && environmentSensor.SlopeAngle == 0 && body.linearVelocityY != 0)) && currentState is GroundMovement)
            {
                ChangeState(airMovement);
            }
            else if (environmentSensor.IsOnGround && currentState is AirMovement && !airMovement.IsJumpBufferActive())
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