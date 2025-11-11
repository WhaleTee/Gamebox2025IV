using Cysharp.Threading.Tasks;
using Misc;
using Reflex.Attributes;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Input
{
    [Serializable]
    public class UserInput
    {
        private SceneLifeCycle lifeCycle;
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

        public UserInput(SceneLifeCycle lifeCycle) => lifeCycle.OnAwake += Enable;
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