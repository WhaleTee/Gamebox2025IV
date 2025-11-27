using UnityEngine;

[System.Serializable]
public class Note
{
    public string Title;   // заголовок для списка
    [TextArea] public string Content; // полный текст
}
