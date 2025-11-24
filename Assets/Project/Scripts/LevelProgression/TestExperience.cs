using UnityEngine;

public class TestExperience : MonoBehaviour
{
    public PlayerLevel player;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("PlayerLevel не назначен в инспекторе!");
            return;
        }

        player.ResetProgress();
    }

    private void Update()
    {
        if (player == null) return;

        if (UnityEngine.Input.GetKeyDown(KeyCode.F))
        {
            player.AddExperience(5);
            Debug.Log("Начислено 5 опыта по нажатию F");
        }
    }
}
