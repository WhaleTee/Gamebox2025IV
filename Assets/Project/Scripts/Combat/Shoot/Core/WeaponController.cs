using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Characters.Equipment;
using Characters;
using Input;
using Phase = UnityEngine.InputSystem.InputActionPhase;
using System.Collections.Generic;

namespace Combat.Weapon
{
    [Serializable]
    public class WeaponController
    {
        private UserInput userInput;
        public int Current { get; private set; }
        private WeaponProjectiled current;
        private CharacterBase owner;

        public void Install(CharacterBase owner, UserInput userInput)
        {
            this.owner = owner;
            this.userInput = userInput;
            SubAll();
        }

        public void OnEnable()
        {
            SubAll();
        }
        public void OnDisable() => UnsubAll();

        private void UseStart()
        {
            if (current is IUsableEquipment usable)
                usable.UseStart();
        }

        private void UseCancel()
        {
            if (current is IUsableEquipment usable)
                usable.UseCancel();
        }

        private void SetCurrentEquipment(int desired)
        {
            List<WeaponProjectiled> weapons = new();

            for (int i = 0; i < owner.Inventory.Equipment.Count; i++)
            {
                var slot = Slot.Weapon;
                var eq = owner.Inventory.Equipment[slot];
                if (i < eq.Count && eq[i] is WeaponProjectiled weapon)
                    weapons.Add(weapon);
            }

            if (weapons.Count < 1)
                return;

            if (desired >= weapons.Count)
                desired = weapons.Count - 1;
            if (desired < 0)
                desired = 0;

            if (Current == desired && current != null)
                return;

            WeaponProjectiled old = current;
            WeaponProjectiled newy = weapons[desired];

            Disable(old);
            Enable(newy);

            Current = desired;
            current = newy;
        }

        public void OnScroll(Vector2 scroll)
        {
            int input = 0;
            if (scroll.y < 0)
                input = -1;
            if (scroll.y > 0)
                input = 1;

            int desired = Current + input;
            SetCurrentEquipment(desired);
        }

        public void OnArrows(Key key)
        {
            int input = -10;
            if (key == Key.Comma)
                input = -1;
            else if (key == Key.Period)
                input = 1;

            SetCurrentEquipment(Current + input);
        }

        public void OnDigits(Key key)
        {
            int inputNumber = key - Key.Digit1;
            SetCurrentEquipment(inputNumber);
        }

        private void Disable(WeaponProjectiled item)
        {
            if (item == null)
                return;

            item.Detach();
            item.gameObject.SetActive(false);
        }

        private void Enable(WeaponProjectiled item)
        {
            if (item == null)
                return;

            var attach = owner.Attaches[item.Slot];
            item.Attach(attach);
            item.gameObject.SetActive(true);
            item.SetActive(true);
        }

        private void SubAll()
        {
            if (userInput == null)
                return;

            userInput.Attack[Phase.Started] += UseStart;
            userInput.Attack[Phase.Canceled] += UseCancel;
            userInput.Scroll += OnScroll;
            userInput.Arrows += OnArrows;
            userInput.Digits += OnDigits;
        }

        private void UnsubAll()
        {
            userInput.Attack[Phase.Started] -= UseStart;
            userInput.Attack[Phase.Canceled] -= UseCancel;
            userInput.Scroll -= OnScroll;
            userInput.Arrows -= OnArrows;
            userInput.Digits -= OnDigits;
        }
    }
}