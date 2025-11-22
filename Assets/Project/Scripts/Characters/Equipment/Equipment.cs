using UnityEngine;
using Misc;

namespace Characters.Equipment
{
    public class Equipment<TConfig, TStats> : MonoBehaviour, IEquipment where TConfig : EquipmentConfig<TStats>
    {
        [field: SerializeField] public Slot Slot { get; protected set; }
        [field: SerializeField] public TConfig Config { get; protected set; }
        [field: SerializeField] public bool Active { get; protected set; }
        [SerializeField] protected ActivateBase m_data;
        protected IActivate activate;

        public virtual void Attach(Transform to)
        {
            activate = m_data;
            transform.SetPositionAndRotation(to.position, to.rotation);
            transform.SetParent(to);
        }

        public virtual void Detach()
        {
            transform.SetParent(null);
        }

        public virtual void SetActive(bool value)
        {
            Active = value;
            if (activate == null)
            {
                Debug.LogError($"Activate is Null!");
                return;
            }
            activate.SetActive(value);
        }
    }
}