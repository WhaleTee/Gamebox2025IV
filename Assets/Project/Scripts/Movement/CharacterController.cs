using UnityEngine;

namespace Project.Scripts.Movement
{
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(CharacterJump))]
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private PresetObject preset;

        private CharacterMovement moveScript;
        private CharacterJump jumpScript;

        private PresetObject installedPreset;

        private void Awake()
        {
            moveScript = GetComponent<CharacterMovement>();
            jumpScript = GetComponent<CharacterJump>();

            InstallPresetData();
        }

        private void FixedUpdate()
        {
            if (installedPreset != preset) InstallPresetData();
        }

        private void InstallPresetData()
        {
            
            moveScript.InstallPreset(preset);
            jumpScript.InstallPreset(preset);

            installedPreset = preset;
        }
    }
}