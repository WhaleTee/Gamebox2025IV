using Combat.Weapon;
using LevelProgression;
using PlayerAbilities = LevelProgression.PlayerAbilities;

namespace Characters
{
    public class Hero : CharacterBase
    {
        private PlayerAbilities playerAbilities;
        private PlayerLevel playerLevel;

        private void Start()
        {
            Init();
            playerAbilities = GetComponent<PlayerAbilities>();
            playerLevel = GetComponent<PlayerLevel>();
            controller.WeaponChanged += OnWeaponChanged;
        }

        private void OnWeaponChanged(WeaponProjectiled newWeapon, WeaponProjectiled oldWeapon)
        {
            playerAbilities.SetBaseDamage(newWeapon.Config.Stats.Damage);
            newWeapon.SetDamage(playerAbilities.DamageBundle);
        }
    }
}
