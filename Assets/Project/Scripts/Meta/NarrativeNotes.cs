using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeNotes", menuName = "Game/Narrative Notes")]
public class NarrativeNotes : ScriptableObject
{
    [System.Serializable]
    public struct NoteData
    {
        [TextArea(3, 15)] public string text;
        public NoteCategory category;
    }

    public NoteData[] notes;
}
