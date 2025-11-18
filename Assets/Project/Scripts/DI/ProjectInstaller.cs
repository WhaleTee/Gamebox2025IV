using System;
using Artifacts;
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

        private void InstallServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(typeof(UserInput));
            containerBuilder.AddSingleton(typeof(SceneService));
            containerBuilder.AddSingleton(typeof(GameObjectFactory), typeof(DeactivatedGameObjectFactory));
            containerBuilder.AddSingleton(typeof(ObjectPoolManager), typeof(ObjectPoolManager), typeof(IInitializable));
            containerBuilder.AddSingleton(typeof(ArtifactInventoryQuickAccess), typeof(ArtifactInventoryQuickAccess), typeof(IInitializable));
            containerBuilder.AddSingleton(typeof(ArtifactInventory));
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(ROOT_CONTAINER_NAME);
            InstallServices(containerBuilder);
        }
    }
}