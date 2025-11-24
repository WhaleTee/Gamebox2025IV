using UnityEngine;
using System;

public class PlayerLevel : MonoBehaviour
{
    [Header("Система уровней")]
    public LevelProgressionSO levelProgression;

    [Header("Модификаторы по уровням")]
    public ModifierSO[] modifiers; // индекс 0 = уровень 1

    public int CurrentExp { get; private set; } = 0;
    public int CurrentLevel { get; private set; } = 0;

    public event Action<int> OnLevelUp;

    private PlayerAbilities abilities;

    private void Awake()
    {
        abilities = GetComponent<PlayerAbilities>();
        CurrentLevel = levelProgression != null ? levelProgression.GetLevel(CurrentExp) : 0;

        // Применяем модификаторы для начального уровня
        for (int lvl = 1; lvl <= CurrentLevel; lvl++)
            ApplyModifierForLevel(lvl);
    }

    public void AddExperience(int amount)
    {
        if (levelProgression == null) return;

        CurrentExp += amount;
        Debug.Log($"Получено опыта: {amount}, всего: {CurrentExp}");

        int newLevel = levelProgression.GetLevel(CurrentExp);

        if (newLevel > CurrentLevel)
        {
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
        if (abilities == null)
            abilities = GetComponent<PlayerAbilities>();

        if (modifiers == null)
        {
            Debug.LogWarning("Массив модификаторов пуст!");
            return;
        }

        int index = level - 1;

        if (index < 0 || index >= modifiers.Length)
        {
            Debug.LogWarning($"Нет модификатора для уровня {level} (индекс {index})");
            return;
        }

        if (modifiers[index] == null)
        {
            Debug.LogWarning($"Модификатор null для уровня {level}");
            return;
        }

        modifiers[index].ApplyTo(abilities);
    }

    public void ResetProgress()
    {
        CurrentExp = 0;
        CurrentLevel = 0;

        if (abilities == null)
            abilities = GetComponent<PlayerAbilities>();

        if (modifiers != null)
        {
            foreach (var mod in modifiers)
            {
                if (mod != null)
                    mod.RemoveFrom(abilities);
            }
        }

        Debug.Log($"Прогресс обнулен. CurrentExp={CurrentExp}, CurrentLevel={CurrentLevel}");
    }
}
