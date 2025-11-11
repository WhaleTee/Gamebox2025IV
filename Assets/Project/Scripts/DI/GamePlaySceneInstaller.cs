using UnityEngine;
using Reflex.Core;
using Spawn;
using GamePlay;
using Characters;
using Misc;

namespace DI
{
    public class GamePlaySceneInstaller : MonoBehaviour, IInstaller
    {
        private string containerName = "GamePlay Scene Container";
        private GamePlaySceneDependencies dependencies;
        private Container container;

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
            dependencies ??= GameObject.FindFirstObjectByType<GamePlaySceneDependencies>(FindObjectsInactive.Include);

            if (dependencies == null)
            {
                Debug.LogError($"Scene dependencies were not found");
                return;
            }

            InstallMessagePipe(containerBuilder);
            InstallDependencies(containerBuilder);

            containerBuilder.OnContainerBuilt += SetContainer;
        }

        public void SetContainer(Container container)
        {
            this.container = container;
        }

        private void Awake()
        {
            container.Resolve<SceneLifeCycle>().Awake();
        }

        private void Start()
        {
            container.Resolve<SceneLifeCycle>().Start();
        }
    }
}