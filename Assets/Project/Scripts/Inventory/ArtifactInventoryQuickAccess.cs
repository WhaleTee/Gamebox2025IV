using Input;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Artifacts
{
    public class ArtifactInventoryQuickAccess : IInitializable
    {
        [Inject] private UserInput userInput;
        [Inject] private ArtifactInventory artifactInventory;
        
        public void Initialize()
        {
            userInput.SubscribeNumKeyPerformed(OnNumKeyPerformed);
            AddFirstArtifact();
        }

        private void AddFirstArtifact()
        {
            artifactInventory.AddArtifact(new Artifact { damageType = DamageType.Stone });
        }

        private void OnNumKeyPerformed(InputAction.CallbackContext ctx)
        {
            artifactInventory.GetArtifact((int)ctx.ReadValue<float>());
        }
    }
}