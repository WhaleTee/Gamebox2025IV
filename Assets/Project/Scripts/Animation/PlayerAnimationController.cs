using Movement;
using UnityEditor.Animations;
using UnityEngine;

namespace Animation
{
    public enum Gender { Male, Female }

    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private AnimatorController male;
        [SerializeField] private AnimatorController female;
        
        private CharacterMovement movementController;
        private Rigidbody2D body;
        private Animator animator;

        private readonly int idleHash = Animator.StringToHash("Idle");
        private readonly int walkHash = Animator.StringToHash("Walk");
        private readonly int jumpRisingHash = Animator.StringToHash("JumpRising");
        private readonly int fallingHash = Animator.StringToHash("Falling");
        private readonly int landingHash = Animator.StringToHash("Landing");
        private readonly int climbingHash = Animator.StringToHash("Climbing");

        private MovementState previousState;
        private MovementState currentState;

        private void Awake()
        {
            movementController = GetComponentInParent<CharacterMovement>();
            body = GetComponentInParent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = PlayerGenderSettings.Instance.Gender == Gender.Female ? female : male;
        }

        private void Start()
        {
            movementController.StateChange += OnMovementStateChange;
        }

        private void Update()
        {
            animator.speed = 1;
            if (currentState is AirMovement)
            {
                var linearVelocityY = body.linearVelocityY - movementController.GroundVelocity.y;
                if (linearVelocityY < 0.1f) animator.Play(fallingHash);
                else if (linearVelocityY > 0.1f) animator.Play(jumpRisingHash);
            }
            else if (currentState is GroundMovement)
            {
                if (movementController.IsSliding) animator.Play(fallingHash);
                else if (Mathf.Abs(body.linearVelocityX - movementController.GroundVelocity.x) > .1f) animator.Play(walkHash);
                else animator.Play(idleHash);
            } else if (currentState is StairsMovement)
            {
                if (body.linearVelocityY != 0) animator.Play(climbingHash);
                else animator.speed = 0;
            }
        }

        private void OnMovementStateChange(MovementState state)
        {
            // if (currentState is AirMovement && state is GroundMovement) animator.Play(landingHash);
            currentState = state;
        }
    }
}