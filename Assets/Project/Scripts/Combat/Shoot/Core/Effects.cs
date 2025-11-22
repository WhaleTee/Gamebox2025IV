using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Combat.Weapon.Audio;
using VisualEffects;

namespace Combat.Weapon
{
    [Serializable]
    public class Effects
    {
        private WeaponAudio audio;
        private WeaponAudio audioInStayOut;
        private VisualEffectsWeapon vfx;
        private Transform firePoint;
        private Events events;
        private Transform transform;
        private Vector3 selfPos => transform.position;

        ~Effects() => UnsubAll();

        public void Install(Transform self, Events events, Transform firePoint, WeaponConfigAudio audioConfig, WeaponConfigVisualEffects vfxConfig)
        {
            this.firePoint = firePoint;
            this.events = events;
            transform = self.transform;
            audio = new();
            audioInStayOut = new();
            vfx = new();

            audio.Install(audioConfig, audioConfig.PolyphonyLimit);
            vfx.Install(vfxConfig);
            audioInStayOut.Install(audioConfig, 1);
        }

        public void Init()
        {
            vfx.Init();
            audio.Init();
            audioInStayOut.Init();
            SubAll();
        }

        public void Stop(Action callback, System.Threading.CancellationToken cancel = default)
        {
            int maxDelay = Mathf.CeilToInt(Mathf.Max(audio.Duration, audioInStayOut.Duration, vfx.Duration) * 1000f);
            UniTask.RunOnThreadPool(async () => { await UniTask.Delay(maxDelay); callback?.Invoke(); }, cancellationToken: cancel);
        }

        private Quaternion NormalToRotation(Vector3 normal)
        {
            var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, 0, angle + 90f);
        }

        private void Sound(Vector2 origin) => audio?.Play(SoundType.Shot, origin, firePoint);
        private void Muzzle(Vector2 origin) => vfx.Play(EffectType.Muzzle, origin, firePoint.rotation);
        private void Impact(Vector2 origin, RaycastHit2D hit) => vfx?.Play(EffectType.Impact, hit.point, NormalToRotation(hit.normal));
        private void SoundExhausted() => audio.Play(SoundType.Exhausted, selfPos, transform);
        private void SoundStart() => audioInStayOut.Play(SoundType.Start, selfPos, transform);
        private void SoundFinish() => audioInStayOut.Play(SoundType.Finish, selfPos, transform);
        private void SoundContinuous() => audio.Play(SoundType.Continuous, selfPos, transform, false);
        private void ImpactProjectile(Vector2 point, Vector3 normal) => vfx?.Play(EffectType.Impact, point, NormalToRotation(normal));

        private void SubAll()
        {
            events.OnShot += Sound;
            events.OnShot += Muzzle;
            events.OnProjectileHit += ImpactProjectile;
            events.OnRayHit += Impact;
            events.OnExhausted += SoundExhausted;
            events.OnStart += SoundStart;
            events.OnContinuous += SoundContinuous;
            events.OnFinish += SoundFinish;
        }

        private void UnsubAll()
        {
            events.OnShot -= Sound;
            events.OnShot -= Muzzle;
            events.OnProjectileHit -= ImpactProjectile;
            events.OnRayHit -= Impact;
            events.OnExhausted -= SoundExhausted;
            events.OnStart -= SoundStart;
            events.OnContinuous -= SoundContinuous;
            events.OnFinish -= SoundFinish;
        }
    }
}