using Factory;
using Pooling;
using UnityEngine;
using Reflex.Core;
using Input;

namespace DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        private const string ROOT_CONTAINER_NAME = "Root Container";
        [SerializeField] private static UserInput m_userInput;

        private Container rootContainer;

        private static void InstallMessagePipe(ContainerBuilder rootContainerBuilder)
        {
        }

        private static void InstallServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(typeof(UserInput));
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