using System;
using System.Collections.Generic;
using Characters.Equipment;
using Extensions;

namespace Characters
{
    public class Inventory
    {
        public event Action<IEquipment> OnEquipmentAdded;
        public Dictionary<Slot, List<IEquipment>> Equipment { get; private set; }

        public Inventory()
        {
            Equipment = new();
            Slot[] s = (Slot[])Enum.GetValues(typeof(Slot));
            Equipment.Populate(s, slot => new());
        }

        public void Add(IEquipment equipment)
        {
            Equipment[equipment.Slot].Add(equipment);
            OnEquipmentAdded?.Invoke(equipment);
        }
    }
}