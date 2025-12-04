using UnityEngine;
using Reflex.Core;
using Extensions;

namespace DI
{
    public class MainMenuInstaller : MonoBehaviour, IInstaller
    {
        private string containerName = "GamePlay Scene Container";
        private MainMenuDependencies dependencies;
        private Container container;

        private void InstallMessagePipe(ContainerBuilder containerBuilder)
        {
        }

        private void InstallDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(dependencies.SoundSettingsData);
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(containerName);

            dependencies ??= GetComponent<MainMenuDependencies>();
            dependencies ??= FindFirstObjectByType<MainMenuDependencies>(FindObjectsInactive.Include);

            if (dependencies == null)
            {
                Debug.LogError($"Scene dependencies were not found");
                return;
            }

            InstallMessagePipe(containerBuilder);
            InstallDependencies(containerBuilder);

            containerBuilder.OnContainerBuilt += OnContainerBuilt;
        }

        private void OnContainerBuilt(Container container)
        {
            this.container = container;
        }

        private void Awake()
        {
            Inject();
            Initialize();
        }

        private void Inject()
        {
            var postInjectables = container.All<IInjectable>();
            foreach (var injectable in postInjectables)
                injectable.InjectAttributes();
        }

        private void Initialize()
        {
            var initializables = container.All<IInitializable>();
            foreach (var initializable in initializables)
                initializable.Initialize();
        }
    }
}