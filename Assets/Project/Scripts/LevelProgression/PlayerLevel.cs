using UnityEngine;
using System;

public class PlayerLevel : MonoBehaviour
{
    [Header("Система уровней")]
    public LevelProgressionSO levelProgression;

    [Header("Модификаторы по уровням")]
    public ModifierSO[] modifiers; // Соответствует уровням 1,2,3...

    public int CurrentExp { get; private set; } = 0;
    public int CurrentLevel { get; private set; } = 0;

    // Событие для UI или других систем
    public event Action<int> OnLevelUp;

    /// <summary>
    /// Добавить опыт
    /// </summary>
    public void AddExperience(int amount)
    {
        if (levelProgression == null) return;

        CurrentExp += amount;
        Debug.Log($"Получено опыта: {amount}, всего: {CurrentExp}");

        int newLevel = levelProgression.GetLevel(CurrentExp);
        if (newLevel > CurrentLevel)
        {
            CurrentLevel = newLevel;
            Debug.Log($"Левел апнут! Новый уровень: {CurrentLevel}");

            OnLevelUp?.Invoke(CurrentLevel);

            ApplyModifierForLevel(CurrentLevel);
        }
    }

    private void ApplyModifierForLevel(int level)
    {
        if (modifiers != null && level - 1 < modifiers.Length && modifiers[level - 1] != null)
        {
            modifiers[level - 1].ApplyModifier();
        }
    }

    public void ResetProgress()
    {
        CurrentExp = 0;
        CurrentLevel = 0;
    }
}
