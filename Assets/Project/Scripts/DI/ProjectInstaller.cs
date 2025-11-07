using UnityEngine;
using Reflex.Core;
using Project.Scripts.Input;

namespace DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        private const string ROOT_CONTAINER_NAME = "Root Container";

        private Container rootContainer;

        private static void InstallMessagePipe(ContainerBuilder rootContainerBuilder)
        {
        }

        private static void InstallServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(typeof(UserInput));
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(ROOT_CONTAINER_NAME);
            InstallMessagePipe(containerBuilder);
            InstallServices(containerBuilder);
            rootContainer = containerBuilder.Build();
        }
    }
}