using UnityEngine;
using UnityEngine.UI;
using Reflex.Core;
using Misc;
using GamePlay;
using Project.Scripts.Input;

namespace DI
{
    public class GamePlaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private string m_containerName = "GamePlay Scene Container";
        [SerializeField] private UserInput m_userInput;
        [Header("Victory Presenter")]
        [SerializeField] private ObservableTrigger m_victoryTrigger;
        [SerializeField] private RectTransform m_victoryDialog;
        [SerializeField] private Button m_victoryCloseButton;

        private Container container;

        private void InstallMessagePipe(ContainerBuilder containerBuilder)
        {
        }

        private void InstallDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(_ => container);
            containerBuilder.AddSingleton(_ => m_userInput);
            containerBuilder.AddSingleton(typeof(Misc.Dummy.SceneService));
            containerBuilder.AddScoped(_ => m_victoryTrigger as ITrigger);
            containerBuilder.AddScoped(_ => m_victoryDialog);
            containerBuilder.AddScoped(_ => m_victoryCloseButton);
        }

        private void Construct()
        {
            var victoryPresenter = container.Construct<ResultPresenter>();
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetName(m_containerName);
            InstallMessagePipe(containerBuilder);
            InstallDependencies(containerBuilder);
            container = containerBuilder.Build();
                
            Construct();
        }
    }
}