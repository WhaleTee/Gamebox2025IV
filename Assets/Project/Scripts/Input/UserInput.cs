using System;
using Project.Scripts.Misc;
using UnityEngine;

namespace Project.Scripts.Input
{
    public class UserInput : Singleton<UserInput>
    {
        private InputActions inputActions;

        public Vector2 Movement => inputActions.Player.Move.ReadValue<Vector2>();
        public bool JumpPerformed => JumpPressed && inputActions.Player.Jump.WasPressedThisDynamicUpdate();
        public bool JumpPressed => inputActions.Player.Jump.IsPressed();
        public bool AttackPerformed => AttackPressed && inputActions.Player.Attack.WasPressedThisDynamicUpdate();
        public bool AttackPressed => inputActions.Player.Attack.IsPressed();

        private void OnEnable() => ActivateInputActions();

        protected override void Awake()
        {
            base.Awake();
            ActivateInputActions();
        }

        private void OnDisable() => DeactivateInputActions();

        private void OnDestroy() => DeactivateInputActions();

        private void ActivateInputActions()
        {
            inputActions ??= new InputActions();
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