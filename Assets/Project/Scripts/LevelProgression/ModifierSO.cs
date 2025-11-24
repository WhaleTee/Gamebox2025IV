using UnityEngine;

public enum ModifierType
{
    DestroyPlant,
    DestroyStone
}

[CreateAssetMenu(fileName = "ModifierSO", menuName = "Modifier")]
public class ModifierSO : ScriptableObject
{
    public string modifierName;
    public ModifierType modifierType;

    public virtual void ApplyTo(PlayerAbilities target)
    {
        if (target == null) return;

        switch (modifierType)
        {
            case ModifierType.DestroyPlant:
                target.AddDamage(DamageType.Plant);
                break;
            case ModifierType.DestroyStone:
                target.AddDamage(DamageType.Stone);
                break;
        }

        Debug.Log($"Модификатор применён: {modifierName} -> {modifierType}");
    }

    public virtual void RemoveFrom(PlayerAbilities target)
    {
        if (target == null) return;

        switch (modifierType)
        {
            case ModifierType.DestroyPlant:
                target.RemoveDamage(DamageType.Plant);
                break;
            case ModifierType.DestroyStone:
                target.RemoveDamage(DamageType.Stone);
                break;
        }

        Debug.Log($"Модификатор удалён: {modifierName} -> {modifierType}");
    }
}
