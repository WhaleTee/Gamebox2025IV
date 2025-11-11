using System;
using UnityEngine;

namespace Input
{
    public class UserInput
    {
        private InputActions inputActions;

        public bool Enabled { get => enabled; set { enabled = value; OnEnabledChanged(value); } }
        public Vector2 Movement => inputActions.Player.Move.ReadValue<Vector2>();
        public bool JumpPerformed => JumpPressed && inputActions.Player.Jump.WasPressedThisDynamicUpdate();
        public bool JumpPressed => inputActions.Player.Jump.IsPressed();
        public bool AttackPerformed => AttackPressed && inputActions.Player.Attack.WasPressedThisDynamicUpdate();
        public bool AttackPressed => inputActions.Player.Attack.IsPressed();
        private bool enabled;

        private void OnEnabledChanged(bool state) { if(state) Enable(); else Disable(); }
        private void Enable() => ActivateInputActions();
        private void Disable() => DeactivateInputActions();

        public UserInput() => Enabled = true;
        
        ~UserInput() => Enabled = false;

        private void ActivateInputActions()
        {
            inputActions ??= new();
            inputActions.Enable();
        }

        private void DeactivateInputActions()
        {
            inputActions?.Player.Disable();
            inputActions?.UI.Disable();
            inputActions?.Disable();
        }
        
        public void SubscribeJumpPerformed(Action callback) => inputActions.Player.Jump.performed +=  _ => callback();
        public void SubscribeJumpCanceled(Action callback) => inputActions.Player.Jump.canceled +=  _ => callback();
    }
}