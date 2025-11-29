using Characters;
using Combat.Weapon;
using Extensions;
using LevelProgression;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ArtifactPickup : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    [SerializeField] private WeaponProjectiled artifact;
    [SerializeField] private DamageModifier damageModifier;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!layers.Contains(collider.gameObject.layer)) return;
        var hero = collider.GetComponent<Hero>();
        EquipWeapon(hero);
        AddModifier(hero);
        Destroy(gameObject);
    }

    private void AddModifier(Hero hero)
    {
        if (hero.TryGetComponent(out PlayerAbilities playerAbilities))
            playerAbilities.AddModifier(damageModifier);
    }

    private void EquipWeapon(CharacterBase owner)
    {
        var weapon = Instantiate(artifact);
        weapon.Install(owner);
        weapon.Init();
        weapon.gameObject.SetActive(false);
        owner.Inventory.Add(weapon);
    }
}