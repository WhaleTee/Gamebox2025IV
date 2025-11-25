using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private Transform notesContainer;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private TextMeshProUGUI selectedNoteText;

    private void OnEnable()
    {
        Journal.Instance.onNoteAdded += AddNoteToUI;

        PopulateNotes();
    }

    private void OnDisable()
    {
        Journal.Instance.onNoteAdded -= AddNoteToUI;
    }

    private void PopulateNotes()
    {
        foreach (Transform child in notesContainer)
            Destroy(child.gameObject);

        foreach (var note in Journal.Instance.Notes)
            AddNoteToUI(note);
    }

    private void AddNoteToUI(Note note)
    {
        GameObject noteGO = Instantiate(notePrefab, notesContainer);
        TMP_Text tmpText = noteGO.GetComponentInChildren<TMP_Text>();
        tmpText.text = note.Title;

        Button btn = noteGO.GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(() => ShowNoteDetail(note));
    }

    private void ShowNoteDetail(Note note)
    {
        selectedNoteText.text = note.Content;
    }
}
