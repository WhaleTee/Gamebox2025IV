using UnityEngine;

/// <summary>
/// Двигающаяся 2D-платформа с плавным движением по AnimationCurve.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Движение")]
    [Tooltip("Скорость цикла движения (секунд на полный путь туда и обратно).")]
    [SerializeField] private float cycleDuration = 2f;

    [Tooltip("Смещение от исходной позиции (в метрах). Например (2,0) — вправо и обратно.")]
    [SerializeField] private Vector2 moveOffset = new Vector2(2f, 0f);

    [Tooltip("Пауза в крайних точках (секунды).")]
    [SerializeField] private float waitTime = 0.3f;

    [Tooltip("Кривая плавности движения (X = 0–1 по пути, Y = позиция 0–1).")]
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float timer;
    private bool movingForward = true;
    private float waitTimer;
    private Vector2 previousPosition;

    public Vector2 Velocity { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        startPosition = rb.position;
        targetPosition = startPosition + moveOffset;
        previousPosition = startPosition;
    }

    private void FixedUpdate()
    {
        Velocity = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        float normalizedTime = Mathf.Clamp01(timer / cycleDuration);
        float curveValue = movementCurve.Evaluate(normalizedTime);
        
        // Определяем текущую позицию
        Vector2 currentPosition = Vector2.Lerp(startPosition, targetPosition, movingForward ? curveValue : 1f - curveValue);

        // Перемещаем платформу
        rb.linearVelocity = (currentPosition - previousPosition) / Time.fixedDeltaTime;
        Velocity = rb.linearVelocity;
        previousPosition = currentPosition;
        timer += Time.deltaTime;

        // Проверяем, завершён ли проход кривой
        if (timer >= cycleDuration)
        {
            timer = 0f;
            movingForward = !movingForward;
            waitTimer = waitTime;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 start = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Vector2 end = start + moveOffset;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(start, 0.05f);
        Gizmos.DrawSphere(end, 0.05f);
    }
#endif
}
