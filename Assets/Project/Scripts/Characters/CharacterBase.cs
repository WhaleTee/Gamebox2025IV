using System.Collections.Generic;
using UnityEngine;
using Reflex.Attributes;
using EquipmentAttach = Characters.Equipment.Attach;
using EquipmentSlot = Characters.Equipment.Slot;
using UserInput = Input.UserInput;
using Combat.Weapon;
using Extensions;

namespace Characters
{
    public class CharacterBase : MonoBehaviour
    {
        [field: SerializeField] public Dictionary<EquipmentSlot, Transform> Attaches { get; protected set; }
        public Inventory Inventory { get; protected set; }
        //public Controller EquipmentController;
        [SerializeField] protected EquipmentAttach[] m_attaches;
        [SerializeField] protected WeaponProjectiled[] mockWeapons;
        protected WeaponController controller;
        private UserInput userInput;

        //private void OnValidate()
        //{
        //    HashSet<EquipmentSlot> slots = new();
        //    List<int> keep = new();

        //    for (int i = 0; i < m_attaches.Length; i++)
        //    {
        //        var slot = m_attaches[i].Slot;
        //        if (slots.Contains(slot))
        //            continue;

        //        slots.Add(m_attaches[i].Slot);
        //        keep.Add(i);
        //    }

        //    var attaches = new EquipmentAttach[keep.Count];
        //    for (int i = 0; i < keep.Count; i++)
        //        attaches[i] = m_attaches[keep[i]];
        //    m_attaches = attaches;
        //}

        [Inject]
        public void Install(UserInput userInput)
        {
            var slots = (EquipmentSlot[])System.Enum.GetValues(typeof(EquipmentSlot));
            Attaches = new();
            Attaches.Populate(slots, slot => m_attaches[(int)slot].Transform);
            controller = new();
            Inventory = new();
            this.userInput = userInput;
        }

        public void Init()
        {
            //EquipmentController.Install(this, userInput);
            controller.Install(this, userInput);
            AddMockWeapons();
            controller.OnEnable();
        }

        private void AddMockWeapons()
        {
            foreach (var prefab in mockWeapons)
            {
                var weapon = Instantiate(prefab);
                weapon.Install(this);
                weapon.Init();
                weapon.gameObject.SetActive(false);
                Inventory.Add(weapon);
            }
        }

        private void OnEnable() => controller?.OnEnable(); //EquipmentController.OnEnable();
        private void OnDisable() => controller.OnDisable(); //EquipmentController.OnDisable();
    }
}