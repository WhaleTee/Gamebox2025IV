using UnityEngine;
using Characters;
using Combat;
using Combat.Weapon;

namespace LevelProgression
{
    public class ExperienceReward : MonoBehaviour
    {
        [SerializeField] private int amount;

        private void Awake()
        {
            var damageable = GetComponent<Damageable>();
            if (damageable != null)
                damageable.Killed += GiveExp;
        }

        public void GiveExp(IDamageSource dmgSource)
        {
            CharacterBase attaker = null;
            if (dmgSource.TryGetComponent<WeaponProjectiled>(out var weapon))
                attaker = weapon.Owner;
            else
                return;

            if (attaker.TryGetComponent<IExperienceReceiver>(out var receiver))
                receiver.AddExperience(amount);
        }
    }
}
