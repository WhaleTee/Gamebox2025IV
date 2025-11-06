using Project.Scripts.Misc;
using UnityEngine;

namespace Project.Scripts.Input
{
    public class UserInput : Singleton<UserInput>
    {
        private readonly InputActions inputActions = new InputActions();

        public Vector2 Movement => inputActions.Player.Move.ReadValue<Vector2>();
        public bool JumpPerformed => inputActions.Player.Jump.IsPressed() && inputActions.Player.Jump.WasPressedThisFrame();
        public bool AttackPerformed => inputActions.Player.Attack.IsPressed() && inputActions.Player.Attack.WasPressedThisFrame();
        public bool AttackPressed => inputActions.Player.Attack.IsPressed();

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
    }
}