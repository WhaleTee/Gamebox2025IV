using UnityEngine;

public enum ModifierType
{
    DestroySmall,
    DestroyLarge,
    // другие типы модификаторов...
}

[CreateAssetMenu(fileName = "ModifierSO", menuName = "Modifier")]
public class ModifierSO : ScriptableObject
{
    public string modifierName;
    [TextArea] public string description;
    public ModifierType modifierType;

    // Применить модификатор
    public virtual void ApplyTo(PlayerAbilities target)
    {
        if (target == null) return;

        switch (modifierType)
        {
            case ModifierType.DestroySmall:
                target.canDestroySmall = true;
                break;
            case ModifierType.DestroyLarge:
                target.canDestroyLarge = true;
                break;
        }

        Debug.Log($"Модификатор применён: {modifierName} -> {modifierType}");
    }

    // удаление эффекта
    public virtual void RemoveFrom(PlayerAbilities target)
    {
        if (target == null) return;

        switch (modifierType)
        {
            case ModifierType.DestroySmall:
                target.canDestroySmall = false;
                break;
            case ModifierType.DestroyLarge:
                target.canDestroyLarge = false;
                break;
        }
    }
}
