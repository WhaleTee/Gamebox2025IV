using System.Linq;
using UnityEngine;
using Combat.Projectiles.Behaviours;

namespace Combat.Projectiles
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {

        [field: SerializeField] public DefaultBehaviourConfig DefaultConfig { get; private set; }
        [SerializeField, Behaviours] public BehavioursContainer BehavioursContainer;

        public bool Has(BehaviourType behaviourType) => BehavioursContainer.Behaviours.Has(behaviourType);

        public Tout TryGet<Tout>() where Tout : ScriptableObject
        {
            return BehavioursContainer.Configs.First(x => x.GetType() == typeof(Tout)) as Tout;
        }
    }
}