namespace Sound
{
    public enum JournalSoundType
    {
        NotePickup,
        NoteTextAppearance
    }

    public class JournalAudioSourceManager : AudioSourceManager<JournalSoundType>
    {

        private void Start()
        {
            Journal.Instance.onNoteAdded += PlaySound;
        }

        private void OnDestroy()
        {
            Journal.Instance.onNoteAdded -= PlaySound;
        }

        private void PlaySound(NoteCategory cat, string str)
        {
            PlayOneShot(JournalSoundType.NotePickup);
        }
    }
}