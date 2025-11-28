using System;
using System.Collections.Generic;
using UnityEngine;
using Misc;

public class Journal : Singleton<Journal>
{
    [SerializeField] private NarrativeNotes narrativeData;

    // Словарь записок по категориям (чтение снаружи)
    public IReadOnlyDictionary<NoteCategory, List<string>> Notes => notesByCategory;

    private readonly Dictionary<NoteCategory, List<string>> notesByCategory = new();
    private readonly Dictionary<NoteCategory, int> nextIndexPerCategory = new();

    public event Action<NoteCategory, string> onNoteAdded;

    protected override void Awake()
    {
        base.Awake();

        // Инициализация словарей
        foreach (NoteCategory cat in Enum.GetValues(typeof(NoteCategory)))
        {
            notesByCategory[cat] = new List<string>();
            nextIndexPerCategory[cat] = 0;
        }
    }

    // Добавление следующей записи категории
    public void AddNote(NoteCategory category)
    {
        if (narrativeData == null || narrativeData.notes.Length == 0) return;

        int index = nextIndexPerCategory[category];

        // Находим первую непрочитанную запись категории
        for (; index < narrativeData.notes.Length; index++)
        {
            var note = narrativeData.notes[index];
            if (note.category == category)
            {
                notesByCategory[category].Add(note.text);
                nextIndexPerCategory[category] = index + 1; // сохраняем индекс
                onNoteAdded?.Invoke(category, note.text);
                NotePopupUI.Instance.Show(note.text);
                return;

            }
        }
    }
}
