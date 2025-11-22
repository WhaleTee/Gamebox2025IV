using UnityEngine;
using System;

public class PlayerLevel : MonoBehaviour
{
    [Header("Система уровней")]
    public LevelProgressionSO levelProgression;

    [Header("Модификаторы по уровням")]
    public ModifierSO[] modifiers; // индекс 0 -> уровень 1

    public int CurrentExp { get; private set; } = 0;
    public int CurrentLevel { get; private set; } = 0;

    public event Action<int> OnLevelUp;

    private PlayerAbilities abilities;

    private void Awake()
    {
        abilities = GetComponent<PlayerAbilities>();
        CurrentLevel = levelProgression != null ? levelProgression.GetLevel(CurrentExp) : 0;

        // Применяем модификаторы для стартового уровня
        for (int lvl = 1; lvl <= CurrentLevel; lvl++)
        {
            ApplyModifierForLevel(lvl);
        }
    }

    public void AddExperience(int amount)
    {
        if (levelProgression == null) return;

        CurrentExp += amount;
        Debug.Log($"Получено опыта: {amount}, всего: {CurrentExp}");

        int newLevel = levelProgression.GetLevel(CurrentExp);
        if (newLevel > CurrentLevel)
        {
            // Применяем модификаторы
            for (int lvl = CurrentLevel + 1; lvl <= newLevel; lvl++)
            {
                CurrentLevel = lvl;
                Debug.Log($"Левел ап: {CurrentLevel}");
                OnLevelUp?.Invoke(CurrentLevel);
                ApplyModifierForLevel(CurrentLevel);
            }
        }
    }

    private void ApplyModifierForLevel(int level)
    {
        int index = level - 1;

        if (abilities == null)
            abilities = GetComponent<PlayerAbilities>();

        if (modifiers != null && index >= 0 && index < modifiers.Length && modifiers[index] != null)
        {
            if (abilities != null)
                modifiers[index].ApplyTo(abilities);
            else
                Debug.LogError("PlayerAbilities не найден! Модификатор не применён.");
        }
    }

    public void ResetProgress()
    {
        if (modifiers != null)
        {
            foreach (var mod in modifiers)
            {
                mod?.RemoveFrom(abilities);
            }
        }
        Debug.Log($"Прогресс обнулен");
        CurrentExp = 0;
        CurrentLevel = 0;
    }
}
