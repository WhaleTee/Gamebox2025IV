using Combat;
using UnityEngine;

/// <summary>
/// Способности игрока: какие типы урона он может наносить
/// </summary>
public class PlayerAbilities : MonoBehaviour
{
    [Header("Типы урона игрока")]
    public Damage damage;

    public bool CanDestroy(DestructibleType type)
    {
        switch (type)
        {
            case DestructibleType.Plant: return damage.Type.Has(DamageType.Plant);
            case DestructibleType.Stone: return damage.Type.Has(DamageType.Stone);
            default: return false;
        }
    }

    public void AddDamage(DamageType type)
    {
        damage.Type |= type;
        Debug.Log($"Добавлен тип урона: {type}");
    }

    public void RemoveDamage(DamageType type)
    {
        damage.Type &= ~type;
        Debug.Log($"Удалён тип урона: {type}");
    }

}
