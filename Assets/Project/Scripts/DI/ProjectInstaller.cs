using UnityEngine;
using Reflex.Core;
using Factory;
using Pooling;
using Input;

namespace DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        private const string ROOT_CONTAINER_NAME = "Root Container";
        private Container rootContainer;

        private static void InstallMessagePipe(ContainerBuilder rootContainerBuilder)
        {
        }

        private void InstallServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(typeof(UserInput));
            containerBuilder.AddSingleton(typeof(GameObjectFactory), typeof(DeactivatedGameObjectFactory));
            containerBuilder.AddSingleton(typeof(ObjectPoolManager), typeof(ObjectPoolManager), typeof(IInitializable));
        }

        private void OnContainerBuilt(Container container)
        {
            this.rootContainer = container;
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(ROOT_CONTAINER_NAME);
            containerBuilder.OnContainerBuilt += OnContainerBuilt;
            InstallMessagePipe(containerBuilder);
            InstallServices(containerBuilder);
        }
    }
}