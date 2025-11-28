using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NotePickup : MonoBehaviour
{
    [SerializeField] private Note note;

    private Collider2D triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Journal.Instance.AddNote(note);
        Destroy(gameObject);
    }
}