using UnityEngine;

[CreateAssetMenu(fileName = "ModifierSO", menuName = "Modifier")]
public class ModifierSO : ScriptableObject
{
    [Tooltip("Название модификатора")]
    public string modifierName;

    [Tooltip("Описание эффекта")]
    [TextArea]
    public string description;

    public void ApplyModifier()
    {
        // Пока просто выводим в консоль
        Debug.Log($"Модификатор применен: {modifierName} - {description}");
    }
}
