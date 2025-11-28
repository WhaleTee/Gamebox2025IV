using Characters;
using Combat.Weapon;
using Extensions;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ArtifactPickup : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    [SerializeField] private WeaponProjectiled artifact;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!layers.Contains(collider.gameObject.layer)) return;
        var weapon = Instantiate(artifact);
        var owner = collider.GetComponent<Hero>();
        weapon.Install(owner);
        weapon.Init();
        weapon.gameObject.SetActive(false);
        owner.Inventory.Add(weapon);
        Destroy(gameObject);
    }
}