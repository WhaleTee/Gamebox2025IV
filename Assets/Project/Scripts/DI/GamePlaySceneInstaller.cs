using UnityEngine;
using Reflex.Core;
using Characters;
using Spawn;

namespace DI
{
    public class GamePlaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private string containerName = "GamePlay Scene Container";
        [SerializeField] private Transform m_playerStart;
        [SerializeField] private Hero m_playerCharacter;

        private Container container;

        private void InstallMessagePipe(ContainerBuilder containerBuilder)
        {
        }

        private void InstallDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddScoped(c => m_playerStart);
            containerBuilder.AddScoped(c => m_playerCharacter);
        }

        private void Construct()
        {
            container.Construct<PlayerSpawn>();
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(containerName);
            InstallMessagePipe(containerBuilder);
            InstallDependencies(containerBuilder);
            container = containerBuilder.Build();
            Construct();
        }
    }
}