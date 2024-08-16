using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public int id;
    public int chapter;

    [Tooltip("대사 캐릭터")]
    public string name;

    [Tooltip("대사 내용")]
    public string[] contexts;

    [Tooltip("이벤트")]
    public string eventFilter;

    [Tooltip("위치")]
    public string location;

    //[Tooltip("초상화")]
    //public string portrait;
}

[System.Serializable]
public class DialogueEvent
{
    public string name;

    public Vector2 line;
    public Dialogue[] dialogues;
}