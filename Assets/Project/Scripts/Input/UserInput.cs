using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class UserInput
    {
        private InputActions inputActions;

        public Dictionary<InputActionPhase, Action> Attack { get; private set; }
        public event Action<Vector2> Scroll;
        public event Action<Key> Arrows;
        public event Action<Key> Digits;
        public event Action Pause;
        public event Action Interaction;


        public bool Enabled { get => enabled; set { enabled = value; OnEnabledChanged(value); } }
        public Vector2 Movement => inputActions.Player.Move.ReadValue<Vector2>();
        public bool JumpPerformed => JumpPressed && inputActions.Player.Jump.WasPressedThisDynamicUpdate();
        public bool JumpPressed => inputActions.Player.Jump.IsPressed();
        public bool AttackPerformed => AttackPressed && inputActions.Player.Attack.WasPressedThisDynamicUpdate();
        public bool AttackPressed => inputActions.Player.Attack.IsPressed();
        private bool enabled;

        private void OnEnabledChanged(bool state) { if (state) Enable(); else Disable(); }
        private void Enable() => ActivateInputActions();
        private void Disable() => DeactivateInputActions();

        public UserInput()
        {
            Enabled = true;
            Attack = new() { { InputActionPhase.Started, null }, { InputActionPhase.Canceled, null } };
        }

        ~UserInput() => Enabled = false;

        public void SetPlayerInputActive(bool state)
        {
            if (inputActions == null) return;

            if (state)
                inputActions.Player.Enable();
            else
                inputActions.Player.Disable();
        }

        private void InteractionHandle(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                Interaction?.Invoke();
        }



        private void ActivateInputActions()
        {
            inputActions ??= new();
            inputActions.Enable();

            SubscribeAll();
        }

        private void DeactivateInputActions()
        {
            inputActions?.Player.Disable();
            inputActions?.UI.Disable();
            inputActions?.Disable();

            UnsubscribeAll();
        }

        public void SubscribeJumpPerformed(Action<InputAction.CallbackContext> callback) => inputActions.Player.Jump.performed += callback;
        public void UnsubscribeJumpPerformed(Action<InputAction.CallbackContext> callback) => inputActions.Player.Jump.performed -= callback;
        public void SubscribeJumpCanceled(Action<InputAction.CallbackContext> callback) => inputActions.Player.Jump.canceled += callback;
        public void UnsubscribeJumpCanceled(Action<InputAction.CallbackContext> callback) => inputActions.Player.Jump.canceled -= callback;

        private void PauseHandle(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                Pause?.Invoke();
        }

        private void AttackHandle(InputAction.CallbackContext context)
        {
            if (Attack.Keys.Contains(context.phase))
                Attack[context.phase]?.Invoke();
        }

        private void ScrollHandle(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                Scroll?.Invoke(context.ReadValue<Vector2>());
        }

        private void ArrowsHandle(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                EmmitKeyCode(context, Arrows);
        }

        private void DigitsHandle(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                EmmitKeyCode(context, Digits);
        }

        private void SubscribeAll()
        {
            inputActions.Player.Attack.started += AttackHandle;
            inputActions.Player.Attack.canceled += AttackHandle;
            inputActions.Player.Scroll.performed += ScrollHandle;
            inputActions.Player.Arrows.performed += ArrowsHandle;
            inputActions.Player.Digits.performed += DigitsHandle;
            inputActions.UI.Pause.performed += PauseHandle;
            inputActions.Player.Interaction.performed += InteractionHandle;

        }

        private void UnsubscribeAll()
        {
            inputActions.Player.Attack.started -= AttackHandle;
            inputActions.Player.Attack.canceled -= AttackHandle;
            inputActions.Player.Scroll.performed -= ScrollHandle;
            inputActions.Player.Arrows.performed -= ArrowsHandle;
            inputActions.Player.Digits.performed -= DigitsHandle;
            inputActions.UI.Pause.performed -= PauseHandle;
            inputActions.Player.Interaction.performed -= InteractionHandle;

        }

        private void EmmitKeyCode(InputAction.CallbackContext context, Action<Key> callback)
        {
            if (context.control is not KeyControl keyControl)
                return;

            callback?.Invoke(keyControl.keyCode);
        }
    }
}