using UnityEngine;

[System.Serializable]
public class Note
{
    public string Title;   // заголовок для списка
    public int Index; // индекс текста в записке
    [TextArea] public string Content; // полный текст
}
