using UnityEngine;

public class TestExperience : MonoBehaviour
{
    public PlayerLevel player;

    private void Start()
    {
        player.ResetProgress();
        // Добавляем опыт по 5 очков каждые 2 секунды
        InvokeRepeating(nameof(GainExp), 1f, 2f);
    }

    void GainExp()
    {
        player.AddExperience(5);
    }
}
