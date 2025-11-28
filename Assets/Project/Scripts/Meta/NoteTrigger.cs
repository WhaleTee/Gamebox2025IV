using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    [Header("Категория записки")]
    public NoteCategory category;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Journal.Instance.AddNote(category);
            Destroy(gameObject);
        }
    }
}
