using Characters;
using Combat.Weapon;
using Extensions;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ArtifactPickup : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    [SerializeField] private WeaponProjectiled artifact;

    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!layers.Contains(collider.gameObject.layer)) return;
        var weapon = Instantiate(artifact);
        weapon.Install();
        weapon.Init();
        weapon.gameObject.SetActive(false);
        collider.GetComponent<Hero>().Inventory.Add(weapon);
        Destroy(gameObject);
    }
}