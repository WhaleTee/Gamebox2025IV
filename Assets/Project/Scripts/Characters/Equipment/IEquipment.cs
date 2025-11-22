using UnityEngine;

namespace Characters.Equipment
{
    public interface IEquipment
    {
        public Slot Slot { get; }

        public void SetActive(bool value);
        public void Attach(Transform to);
        public void Detach();
    }
}