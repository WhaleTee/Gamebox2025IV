using UnityEngine;

namespace Movement
{
    [CreateAssetMenu(menuName = "Character/Movement Preset", fileName = " - Character Movement Preset")]
    public class PresetObject : ScriptableObject
    {
        
        [Header("Ground Movement")]
        [field: SerializeField] public GroundMovementSettings GroundMovementSettings { get; private set; }
        [field: SerializeField] public AirMovementSettings AirMovementSettings { get; private set; }
        [field: SerializeField] public string PresetName { get; private set; }
    }
}