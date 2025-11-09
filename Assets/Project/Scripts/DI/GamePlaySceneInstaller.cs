using UnityEngine;
using Reflex.Core;

namespace DI
{
    public class GamePlaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private string containerName = "GamePlay Scene Container";

        private Container rootContainer;

        private static void InstallMessagePipe(ContainerBuilder rootContainerBuilder)
        {
        }

        private static void InstallServices(ContainerBuilder containerBuilder)
        {
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(containerName);
            InstallMessagePipe(containerBuilder);
            InstallServices(containerBuilder);
        }
    }
}