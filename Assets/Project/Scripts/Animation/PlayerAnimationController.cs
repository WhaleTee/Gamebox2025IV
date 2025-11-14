using Movement;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private CharacterMovement movementController;
        private Rigidbody2D body;
        private Animator animator;

        private readonly int idleHash = Animator.StringToHash("Idle");
        private readonly int walkHash = Animator.StringToHash("Walk");
        private readonly int jumpRisingHash = Animator.StringToHash("JumpRising");
        private readonly int fallingHash = Animator.StringToHash("Falling");
        private readonly int landingHash = Animator.StringToHash("Landing");

        private MovementState previousState;
        private MovementState currentState;

        private void Awake()
        {
            movementController = GetComponentInParent<CharacterMovement>();
            body = GetComponentInParent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            movementController.StateChange += OnMovementChange;
        }

        private void Update()
        {
            if (currentState is AirMovement)
            {
                if (body.linearVelocityY < 0) animator.Play(fallingHash);
                else if (body.linearVelocityY > 0) animator.Play(jumpRisingHash);
            }
            else if (currentState is GroundMovement)
            {
                if (body.linearVelocityX - movementController.GroundVelocity.x != 0) animator.Play(walkHash);
                else animator.Play(idleHash);
            }
        }

        private void OnMovementChange(MovementState state)
        {
            if (currentState is AirMovement && state is GroundMovement) animator.Play(landingHash);
            currentState = state;
        }
    }
}