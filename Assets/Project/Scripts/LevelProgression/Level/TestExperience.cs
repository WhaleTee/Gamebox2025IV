using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace LevelProgression
{
    public class TestExperience : MonoBehaviour
    {
        private PlayerLevel player;

        private void Start()
        {
            CancellationTokenSource cts = new();
            UniTask.Action(cts.Token, SeekForPlayerOnScene).Invoke();
        }

        private void Update()
        {
            if (player == null)
                return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                player.AddExperience(5);
                Debug.Log("Начислено 5 опыта по нажатию F");
            }
        }

        private async UniTaskVoid SeekForPlayerOnScene(CancellationToken cts)
        {
            while (player == null && !cts.IsCancellationRequested)
            {
                await UniTask.Delay(400);
                player = FindFirstObjectByType<PlayerLevel>(FindObjectsInactive.Exclude);
            }
        }
    }
}