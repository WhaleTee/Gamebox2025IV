using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Projectiles
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/ProjectileRepository")]
    public class ProjectilesRepository : ScriptableObject
    {
        public ProjectileKVP this[Variant index] => datas[index];
        [SerializeField] private List<ProjectileKVP> m_datas;
        private Dictionary<Variant, ProjectileKVP> datas;

        private void OnEnable()
        {
            datas = new();

            foreach (var item in m_datas)
                datas.Add(item.Key, item);
        }
    }

    [Serializable]
    public class ProjectileKVP
    {
        [field: SerializeField] public Variant Key { get; private set; }
        [field: SerializeField] public ProjectileConfig Config { get; private set; }
        [field: SerializeField] public Projectile Prefab;
    }
}