using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(DamageableObject))]
    public class DestroyOnDeath : MonoBehaviour
    {
        private DamageableObject damageableObject;

        private void Awake()
        {
            damageableObject = GetComponent<DamageableObject>();
        }

        private void Start()
        {
            damageableObject.OnDeath += OnDeath;
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}