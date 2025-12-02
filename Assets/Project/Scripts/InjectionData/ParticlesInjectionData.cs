using System;
using UnityEngine;
using Reflex.Attributes;
using Reflex.Extensions;
using Type = Pooling.PoolType;
using Pool = Pooling.ObjectPoolManager;

namespace VisualEffects
{
    [Serializable]
    public class ParticlesInjectionData : IInjectable
    {
        [Inject] private Pool pool;
        public Action<ParticleSystem> Release => IRelease;
        public Func<ParticleSystem, ParticleSystem> Get => IGet;

        private ParticleSystem IGet(ParticleSystem prefab)
        {
            if (pool == null) pool = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetSceneContainer().Resolve<Pool>();
            return pool.SpawnObject(prefab, Vector3.one* 99999, Quaternion.identity, Type.ParticleSystems);
        }
        private void IRelease(ParticleSystem effect) => pool.ReturnObjectToPool(effect.gameObject, Type.ParticleSystems);
    }
}