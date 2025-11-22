using UnityEngine;

public class TestExperience : MonoBehaviour
{
    public PlayerLevel player;

    private void Awake()
    {
        if (player == null)
        {
            Debug.LogError("PlayerLevel не назначен в инспекторе!");
            return;
        }

        player.ResetProgress();

        // Добавляем опыт по 5 очков каждые 2 секунды
        InvokeRepeating(nameof(GainExp), 1f, 2f);
    }

    void GainExp()
    {
        player.AddExperience(5);
    }
}
