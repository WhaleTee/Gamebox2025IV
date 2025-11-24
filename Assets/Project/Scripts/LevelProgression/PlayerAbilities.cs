using UnityEngine;

/// <summary>
/// Способности игрока: какие типы урона он может наносить
/// </summary>
public class PlayerAbilities : MonoBehaviour
{
    [Header("Типы урона игрока")]
    public DamageType damageType;

    public bool CanDestroy(DestructibleType type)
    {
        switch (type)
        {
            case DestructibleType.Plant: return damageType.Has(DamageType.Plant);
            case DestructibleType.Stone: return damageType.Has(DamageType.Stone);
            default: return false;
        }
    }

    public void AddDamage(DamageType type)
    {
        damageType.Add(type);
        Debug.Log($"Добавлен тип урона: {type}");
    }

    public void RemoveDamage(DamageType type)
    {
        damageType.Remove(type);
        Debug.Log($"Удалён тип урона: {type}");
    }
}
