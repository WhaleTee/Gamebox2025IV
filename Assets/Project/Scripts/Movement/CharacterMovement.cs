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
        private GroundChecker groundChecker;

        private GroundMovement groundMovement;
        private AirMovement airMovement;
        private IMovementState currentState;

        private bool desiredJump;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            groundChecker = GetComponent<GroundChecker>();
        }

        private void Start()
        {
            groundMovement = new GroundMovement(userInput, preset, body, groundChecker);
            airMovement = new AirMovement(userInput, preset, body, groundChecker);
            ChangeState(groundMovement);
            userInput.SubscribeJumpPerformed(OnJumpPerformed);
        }
        
        private void Update()
        {
            if (userInput.Movement.x != 0) transform.localScale = new Vector3(userInput.Movement.x > 0 ? 1 : -1, 1, 1);
            groundMovement.Update();
            airMovement.Update();
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
            if ((!groundChecker.IsOnGround || (groundChecker.SlopeAngle < preset.GroundMovementSettings.maxSlopeAngle && desiredJump) || (groundChecker.SlopeAngle == 0 && body.linearVelocityY != 0)) && currentState is GroundMovement)
            {
                ChangeState(airMovement);
            } else if (groundChecker.IsOnGround && currentState is AirMovement && !airMovement.IsJumpBofferActive()) ChangeState(groundMovement);
        }

        private void ChangeState(IMovementState state)
        {
            currentState?.Exit();
            currentState = state;
        }

        private void OnJumpPerformed(InputAction.CallbackContext ctx) => desiredJump = true;
    }
}