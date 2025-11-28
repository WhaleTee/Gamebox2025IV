namespace Sound
{
    public enum JournalSoundType
    {
        NotePickup,
        NoteTextAppearance
    }
    
    public class JournalAudioSourceManager: AudioSourceManager<JournalSoundType> {
        
        private void Start()
        {
            Journal.Instance.onNoteAdded += _ => PlayOneShot(JournalSoundType.NotePickup);
        }
    }
}