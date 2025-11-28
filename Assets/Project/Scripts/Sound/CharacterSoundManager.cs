using Movement;
using UnityEngine;

namespace Sound
{
    public enum CharacterSoundType
    {
        Footstep,
        Jump,
        Shot,
        Climb,
        
    }

    public class CharacterSoundManager : AudioSourceManager<CharacterSoundType>
    {
        private CharacterMovement movementController;
        private Rigidbody2D body;
        private MovementState currentState;
        
        protected override void Awake()
        {
            base.Awake();
            movementController = GetComponentInParent<CharacterMovement>();
            body = GetComponentInParent<Rigidbody2D>();
        }
        
        private void Start()
        {
            movementController.StateChange += OnMovementStateChange;
        }

        private void Update()
        {
        }

        private void OnMovementStateChange(MovementState state)
        {
            currentState = state;
            if (currentState is AirMovement && movementController.desiredJump) PlayOneShot(CharacterSoundType.Jump);
        }
    }
}