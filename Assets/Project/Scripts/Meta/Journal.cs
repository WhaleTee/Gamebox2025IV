using System;
using System.Collections.Generic;
using UnityEngine;
using Misc;

public class Journal : Singleton<Journal>
{
    public List<Note> Notes { get; private set; } = new List<Note>();

    public event Action<Note> onNoteAdded;

    public void AddNote(Note note)
    {
        Notes.Add(note);
        onNoteAdded?.Invoke(note);
    }
}
