using Characters;
using Characters.Equipment;
using Extensions;
using Input;
using System;
using System.Collections.Generic;
using UnityEngine;
using IEquipment = Characters.Equipment.IEquipment;
using Key = UnityEngine.InputSystem.Key;
using Phase = UnityEngine.InputSystem.InputActionPhase;
using Slot = Characters.Equipment.Slot;

namespace Combat.Weapon
{
    [Serializable]
    public class Controller
    {
        private UserInput userInput;
        public Dictionary<Slot, int> Currents { get; private set; }
        private Dictionary<Slot, IEquipment> currents;
        private CharacterBase owner;

        public void Install(CharacterBase owner, UserInput userInput)
        {
            this.owner = owner;
            this.userInput = userInput;
            var slots = (Slot[])Enum.GetValues(typeof(Slot));
            Currents = new();
            currents = new();
            Currents.Populate(slots);
            currents.Populate(slots);
            SubAll();
        }
        
        public void OnEnable()
        {
            SubAll();
            if (owner == null)
                return;

            var slots = (Slot[])Enum.GetValues(typeof(Slot));
            foreach (var slot in slots)
            {
                var slotItems = owner.Inventory.Equipment[slot];
                if (slotItems.Count > 0)
                    currents[slot] = slotItems[Currents[slot]];
            }
        }
        public void OnDisable() => UnsubAll();

        private void UseStart()
        {
            if (currents[Slot.Weapon] is IUsableEquipment usable)
                usable.UseStart();
        }

        private void UseCancel()
        {
            if (currents[Slot.Weapon] is IUsableEquipment usable)
                usable.UseCancel();
        }

        private void SetCurrentEquipment(Slot slot, int desired)
        {
            var equipments = owner.Inventory.Equipment[slot];

            if (equipments.Count < 1)
                return;

            if (desired >= equipments.Count)
                desired = equipments.Count - 1;
            if (desired < 0)
                desired = 0;

            if (Currents[slot] == desired)
                return;

            IEquipment old = currents[slot];
            IEquipment newy = equipments[desired];

            Disable(old);
            Enable(newy);

            Currents[slot] = desired;
        }

        public void OnScroll(Vector2 scroll)
        {
            int input = 0;
            if (scroll.y < 0)
                input = -1;
            if (scroll.y > 0)
                input = 1;

            int desired = Currents[Slot.Weapon] + input;
            SetCurrentEquipment(Slot.Weapon, desired);
        }

        public void OnArrows(Key key)
        {
            int input = -10;
            if (key == Key.Comma)
                input = -1;
            else if (key == Key.Period)
                input = 1;

            SetCurrentEquipment(Slot.Weapon, Currents[Slot.Weapon] + input);
        }

        public void OnDigits(Key key)
        {
            int inputNumber = key - Key.Digit1;
            SetCurrentEquipment(Slot.Weapon, inputNumber);
        }

        private void Disable(IEquipment item) => item?.SetActive(false);

        private void Enable(IEquipment item)
        {
            if (item == null)
                return;

            var attach = owner.Attaches[item.Slot];
            item.Attach(attach);
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