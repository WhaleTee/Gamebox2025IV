using UnityEngine;

namespace Movement
{
    [CreateAssetMenu(menuName = "Character/Movement Preset", fileName = " - Character Movement Preset")]
    public class PresetObject : ScriptableObject
    {
        [field: SerializeField] public StairsMovementSettings StairsMovementSettings { get; private set; }
        [field: SerializeField] public GroundMovementSettings GroundMovementSettings { get; private set; }
        [field: SerializeField] public AirMovementSettings AirMovementSettings { get; private set; }
    }
}