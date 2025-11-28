using UnityEngine;
using UnityEngine.InputSystem;
using Reflex.Attributes;
using Input;
using Combat;

namespace Artifacts
{
    public class ArtifactInventoryQuickAccess : IInitializable
    {
        [Inject] private UserInput userInput;
        [Inject] private ArtifactInventory artifactInventory;
        
        public void Initialize()
        {
            // userInput.SubscribeNumKeyPerformed(OnNumKeyPerformed);
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