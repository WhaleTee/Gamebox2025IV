using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
namespace Input
{
    public static class InputActionsExtensions
    {
        private static Dictionary<ActionType, InputAction> map;
        private static Dictionary<ActionType, InputAction> InitMap(this InputActions.PlayerActions actions) => map ??= new() {
                { ActionType.Attack, actions.Attack },
                { ActionType.Scroll, actions.Scroll },
                { ActionType.Arrows, actions.Arrows },
                { ActionType.Digits, actions.Digits }
            };

        public static void SetSubscription(this InputActions.PlayerActions actions, ActionType type, Action<InputAction.CallbackContext> callback, bool bindFlag, InputActionPhase phase)
        {
            actions.InitMap();
            map[type].SetSubscription(callback, bindFlag, phase);
        }

        private static void SetSubscription(this InputAction action, Action<InputAction.CallbackContext> callback, bool bindFlag, InputActionPhase phase)
        {
            switch (phase)
            {
                case InputActionPhase.Started:
                    if (bindFlag) action.started += callback;
                    else action.started -= callback;
                    break;
                case InputActionPhase.Performed:
                    if (bindFlag) action.performed += callback;
                    else action.performed -= callback;
                    break;
                case InputActionPhase.Canceled:
                    if (bindFlag) action.canceled += callback;
                    else action.canceled -= callback;
                    break;
                default:
                    break;
            }
        }
    }
}
*/