using Factory;
using Input;
using Misc.Dummy;
using Pooling;
using Reflex.Core;
using UnityEngine;

namespace DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        private const string ROOT_CONTAINER_NAME = "Root Container";

        private static void InstallMessagePipe(ContainerBuilder rootContainerBuilder)
        {
        }

        private void InstallServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(typeof(UserInput));
            containerBuilder.AddSingleton(typeof(SceneService));
            containerBuilder.AddSingleton(typeof(GameObjectFactory), typeof(DeactivatedGameObjectFactory));
            containerBuilder.AddSingleton(typeof(ObjectPoolManager));
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(ROOT_CONTAINER_NAME);
            InstallMessagePipe(containerBuilder);
            InstallServices(containerBuilder);
        }
    }
}