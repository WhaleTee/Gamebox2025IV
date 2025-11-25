using UnityEngine;

public class NotePickup : MonoBehaviour
{
    public string noteTitle;
    [TextArea] public string noteContent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Note note = new Note
            {
                Title = noteTitle,
                Content = noteContent
            };
            Journal.Instance.AddNote(note);

            Destroy(gameObject);
        }
    }
}
