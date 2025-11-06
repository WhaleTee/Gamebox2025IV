using Project.Scripts.Misc;
using UnityEngine;

namespace Project.Scripts.Input
{
    public class UserInput : Singleton<UserInput>
    {
        private InputActions inputActions;

        public Vector2 Movement => inputActions.Player.Move.ReadValue<Vector2>();
        public bool JumpPerformed => inputActions.Player.Jump.IsPressed() && inputActions.Player.Jump.WasPressedThisFrame();
        public bool JumpPressed => inputActions.Player.Jump.IsPressed();
        public bool AttackPerformed => inputActions.Player.Attack.IsPressed() && inputActions.Player.Attack.WasPressedThisFrame();
        public bool AttackPressed => inputActions.Player.Attack.IsPressed();

        private void OnEnable() => ActivateInputActions();

        private void Start()
        {
            DeactivateInputActions();
            ActivateInputActions();
        }

        private void OnDisable() => DeactivateInputActions();

        private void ActivateInputActions()
        {
            inputActions = new InputActions();
            inputActions.Enable();
        }

        private void DeactivateInputActions()
        {
            inputActions?.Player.Disable();
            inputActions?.UI.Disable();
            inputActions?.Disable();
            inputActions?.Dispose();
        }
    }
}