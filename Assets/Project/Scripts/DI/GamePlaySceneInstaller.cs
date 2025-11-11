using Reflex.Core;
using UnityEngine;

namespace DI
{
    public class GamePlaySceneInstaller : MonoBehaviour, IInstaller
    {
        private string containerName = "GamePlay Scene Container";
        private GamePlaySceneDependencies dependencies;

        private void InstallMessagePipe(ContainerBuilder containerBuilder)
        {
        }

        private void InstallDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(dependencies.PlayerData);
            containerBuilder.AddSingleton(dependencies.VictoryData);
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(containerName);

            dependencies ??= GetComponent<GamePlaySceneDependencies>();
            dependencies ??= FindFirstObjectByType<GamePlaySceneDependencies>(FindObjectsInactive.Include);

            if (dependencies == null)
            {
                Debug.LogError($"Scene dependencies were not found");
                return;
            }

            InstallMessagePipe(containerBuilder);
            InstallDependencies(containerBuilder);
        }
    }
}