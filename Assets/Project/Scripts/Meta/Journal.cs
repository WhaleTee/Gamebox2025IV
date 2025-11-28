using System;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class Journal : Singleton<Journal>
{
    [field: SerializeField] public List<Note> Notes { get; private set; }
    public event Action<Note> onNoteAdded;

    public void AddNote(Note note) {
        Notes.Add(note);
        onNoteAdded?.Invoke(note);
    }
}