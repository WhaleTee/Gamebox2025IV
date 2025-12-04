using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JournalUI : MonoBehaviour
{
    [Header("Категория ScrollView")]
    public NoteCategory category;

    [Header("UI")]
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject noteEntryPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private void OnEnable()
    {
        Journal.Instance.onNoteAdded += OnNoteAdded;
        PopulateExistingNotes();
    }

    private void OnDisable()
    {
        Journal.Instance.onNoteAdded -= OnNoteAdded;
    }

    private void PopulateExistingNotes()
    {
        foreach (Transform child in contentContainer)
            Destroy(child.gameObject);

        foreach (var note in Journal.Instance.Notes[category])
            AddNoteToUI(note);
    }

    private void OnNoteAdded(NoteCategory cat, string text)
    {
        if (cat != category) return;
        AddNoteToUI(text);
    }

    private void AddNoteToUI(string noteText)
    {
        TMP_Text tmp = Instantiate(noteEntryPrefab, contentContainer)
                        .GetComponentInChildren<TMP_Text>();
        tmp.text = noteText;

        StartCoroutine(ScrollToBottomNextFrame());
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return null;
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
