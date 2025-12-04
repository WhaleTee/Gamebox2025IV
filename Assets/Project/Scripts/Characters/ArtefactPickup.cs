using Characters;
using Combat.Weapon;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using LevelProgression;
using System.Threading;
using UnityEngine;

[UnityEngine.Scripting.APIUpdating.MovedFrom("ArtifactPickup")]
[RequireComponent(typeof(Collider2D))]
public class ArtefactPickup : MonoBehaviour
{
    [SerializeField] [UnityEngine.Serialization.FormerlySerializedAs("layers")] private LayerMask m_layers;
    [SerializeField] [UnityEngine.Serialization.FormerlySerializedAs("artifact")] private WeaponProjectiled m_artefact;
    [SerializeField] [UnityEngine.Serialization.FormerlySerializedAs("damageModifier")] private DamageModifier m_damageModifier;
    [SerializeField] private int m_activateSlotWeapon;
    
    [Header("Animation")]
    
    [Header("Folow phase")]
    [SerializeField] private float m_startSpeed = 2f;
    [SerializeField] private float m_acceleration = 1.4f;
    [SerializeField] private float m_maxSpeed = 25;
    [SerializeField] private float m_arriveDistance = 0.1f;
    [SerializeField] private float m_deadlineDuration = 5f;

    [Header("Shrink phase")]
    [SerializeField] private float m_originScale = 1f;
    [SerializeField] private float m_shrinkedScale = 0.2f;
    [SerializeField] private float m_secondPhaseDuration = 1f;
    [SerializeField] private float m_accumulationAcceleration = 0.3f;
    [SerializeField] private float m_maxAccum = 3f;
    private float accumulatedSpeed = 1f;
    private Sequence shrinkSequence; 

    private ParticleSystem[] particles;
    private string tweenID = "Pickup_";

    private Collider2D _collider;
    private CancellationTokenSource cts;
    private Transform target;

    private void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
        DOTween.Kill(this);

        if (shrinkSequence != null)
        {
            shrinkSequence.Kill();
            shrinkSequence = null;
        }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        tweenID += gameObject.name;
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!m_layers.Contains(collider.gameObject.layer)) return;
        Hero hero = collider.GetComponent<Hero>();
        if (hero == null) return;
        target = hero.Attaches[Characters.Equipment.Slot.Artefact];
        BeginPickUp(hero);
    }

    private void BeginPickUp(Hero hero)
    {
        cts?.Cancel();
        cts = new();
        PickUp(hero).Forget();
    }

    private async UniTaskVoid PickUp(Hero hero)
    {
        var timeoutTask = UniTask.Delay(System.TimeSpan.FromSeconds(m_deadlineDuration), cancellationToken: cts.Token);
        var animationTask = Animate();
        await UniTask.WhenAny(animationTask, timeoutTask);
        cts.Cancel();
        Consume(hero);
    }
    
    private async UniTask Animate()
    {
        MoveToTargetAsync().Forget();
        await UniTask.WaitUntil(
            () => shrinkSequence != null && shrinkSequence.ElapsedPercentage() >= 1f,
            cancellationToken: cts.Token
        );
    }

    private void Shrink()
    {
        if (shrinkSequence != null)
            return;

        shrinkSequence = DOTween.Sequence();
        shrinkSequence.SetId(tweenID).SetTarget(this).SetAutoKill(false).Pause();

        shrinkSequence.Join(transform.DOScale(Vector3.one * m_shrinkedScale, m_secondPhaseDuration));

        if (particles != null)
            foreach (var p in particles)
            {
                var r = p.GetComponent<ParticleSystemRenderer>();
                if (r != null && r.material.HasColor("_Color"))
                {
                    var c = r.material.color;
                    shrinkSequence.Join(r.material.DOColor(new(c.r, c.g, c.b, 0), m_secondPhaseDuration));
                }
                else
                {
                    shrinkSequence.Join(
                        DOVirtual.DelayedCall(m_secondPhaseDuration,
                        () => p.Stop(true, ParticleSystemStopBehavior.StopEmitting)));
                }
            }
    }

    private void OnTargetEnter()
    {
        Shrink();

        accumulatedSpeed = Mathf.Min(accumulatedSpeed + Time.deltaTime, m_maxAccum);
        shrinkSequence.timeScale = accumulatedSpeed;

        shrinkSequence.PlayForward();
    }

    private void OnTargetExit()
    {
        if (shrinkSequence != null)
            shrinkSequence.PlayBackwards();
    }

    private async UniTask MoveToTargetAsync()
    {
        float speed = m_startSpeed;
        bool entred = false;

        while (!cts.Token.IsCancellationRequested)
        {
            if (target == null)
                return;

            Vector3 toTarget = target.position - transform.position;
            float distance = toTarget.magnitude;

            if (distance < m_arriveDistance && !entred)
            {
                entred = true;
                OnTargetEnter();
            }
            else if (distance > m_arriveDistance && entred)
            {
                entred = false;
                OnTargetExit();
            }

            if (distance == 0)
                distance = float.Epsilon;

                Vector3 dir = toTarget / distance;

            float step = speed * Time.deltaTime;

            if (step > distance) step = distance;

            transform.position += dir * step;

            speed = Mathf.Min(m_maxSpeed, speed + m_acceleration * Time.deltaTime);

            accumulatedSpeed += Time.deltaTime * m_accumulationAcceleration;

            if (shrinkSequence != null)
            shrinkSequence.timeScale = Mathf.Clamp(accumulatedSpeed, 1f, m_maxAccum);

            await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
        }
    }

    private void Consume(Hero hero)
    {
        AddWeapon(hero);
        EquipWeapon(hero);
        AddModifier(hero);
        Destroy(gameObject);
    }

    private void AddModifier(Hero hero)
    {
        if (hero.TryGetComponent(out PlayerAbilities playerAbilities))
            playerAbilities.AddModifier(m_damageModifier);
    }

    private WeaponProjectiled AddWeapon(CharacterBase owner)
    {
        var weapon = Instantiate(m_artefact);
        weapon.Install(owner);
        weapon.Init();
        weapon.gameObject.SetActive(false);
        owner.Inventory.Add(weapon);
        return weapon;
    }

    private void EquipWeapon(CharacterBase owner)
    {
        owner.Controller.SetCurrent(m_activateSlotWeapon);
    }
}