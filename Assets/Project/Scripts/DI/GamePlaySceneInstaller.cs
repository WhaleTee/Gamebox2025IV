using UnityEngine;
using Reflex.Core;
using Extensions;
using Misc;
using Sound;
using Combat.Projectiles.Behaviours;
using VisualEffects;

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
            containerBuilder.AddSingleton(dependencies.UpdatesInjectionData.UpdateRunner, typeof(IUpdateRunner));
            BehavioursSystem behaviourSystem = new();
            containerBuilder.AddSingleton(behaviourSystem, typeof(BehavioursSystem), typeof(IInjectable), typeof(IInitializable));
            containerBuilder.AddSingleton(dependencies.PlayerData);
            containerBuilder.AddSingleton(dependencies.VictoryData);
            containerBuilder.AddSingleton(dependencies.CameraInjectionData);
            containerBuilder.AddSingleton(dependencies.SoundSettingsData);
            containerBuilder.AddSingleton(dependencies.AudioData, typeof(AudioInjectionData), typeof(IInjectable));
            containerBuilder.AddSingleton(dependencies.ParticlesData, typeof(ParticlesInjectionData), typeof(IInjectable));
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