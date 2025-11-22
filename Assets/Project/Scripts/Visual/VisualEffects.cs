using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Reflex.Attributes;
using Reflex.Extensions;
using Pooling;

namespace VisualEffects
{
    [Serializable]
    public class VisualEffectsWeapon
    {
        public float Duration
        {
            get
            {
                float maxMuzzle = GetMaxRemain(effects[EffectType.Muzzle]);
                float maxImpact = GetMaxRemain(effects[EffectType.Impact]);
                return Mathf.Max(maxMuzzle, maxImpact);
            }
        }
        [Inject] private ObjectPoolManager pool;
        private WeaponConfigVisualEffects config;
        private Dictionary<EffectType, Queue<ParticleSystem>> effects;

        public void Inject(ObjectPoolManager pool)
        {
            this.pool = pool;
        }

        public void Install(WeaponConfigVisualEffects config)
        {
            this.config = config;
            effects = new();
            Inject(SceneManager.GetActiveScene().GetSceneContainer().Resolve<ObjectPoolManager>());
        }

        public void Init()
        {
            for (int i = 0; i < config.Effects.Count; i++)
            {
                var kvp = config.Effects[i];
                effects.Add(kvp.Type, new());
            }
        }

        public void Play(EffectType type, Vector2 spawnPoint, Quaternion rotation)
        {
            if (effects[type].Count < 1)
                effects[type].Enqueue(Create(type));
            var effect = effects[type].Peek();
            if (effect == null)
                effect = Create(type);

            effect.transform.SetPositionAndRotation(spawnPoint, rotation);
            effect.Play();
        }

        private ParticleSystem Create(EffectType type)
        {
            var kvp = config.Effects[(int)type];
            ParticleSystem prefab = kvp[-1];
            var effect = pool.SpawnObject(prefab, Vector3.one * 99999f, Quaternion.identity, Pooling.PoolType.ParticleSystems);
            effects[kvp.Type].Enqueue(effect);
            return effect;
        }

        private float GetMaxRemain(IEnumerable<ParticleSystem> values)
        {
            if (values == null)
                return 0;

            static float GetRemain(ParticleSystem ps)
            { var main = ps.main; return main.loop ? main.duration : main.duration - ps.time; }

            float max = 0;
            foreach (var ps in values)
            {
                if (ps == null || !ps.isPlaying)
                    continue;

                float remaining = GetRemain(ps);
                if (remaining > max)
                    max = remaining;
            }

            return max;
        }
    }
}