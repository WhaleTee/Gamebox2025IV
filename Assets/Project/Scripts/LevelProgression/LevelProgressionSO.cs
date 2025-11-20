using UnityEngine;

[CreateAssetMenu(fileName = "LevelProgressionSO", menuName = "Level Progression")]
public class LevelProgressionSO : ScriptableObject
{
    [Tooltip("Список опыта, необходимого для каждого уровня")]
    public int[] experiencePerLevel;

    /// <summary>
    /// Получить уровень по количеству опыта
    /// </summary>
    public int GetLevel(int currentExp)
    {
        int level = 0;
        for (int i = 0; i < experiencePerLevel.Length; i++)
        {
            if (currentExp >= experiencePerLevel[i])
                level = i + 1;
            else
                break;
        }
        return level;
    }

    /// <summary>
    /// Получить опыт для следующего уровня
    /// </summary>
    public int GetExpForNextLevel(int currentLevel)
    {
        if (currentLevel < experiencePerLevel.Length)
            return experiencePerLevel[currentLevel];
        return -1; // максимальный уровень
    }
}
