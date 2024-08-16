using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : GenericSingleton<DatabaseManager>
{
    // csv 파일을 통한 Dialogue 출력
    [SerializeField] string csv_FileName;
    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();


    protected override void Awake()
    {
        base.Awake();
        DialogueParser theParser = GetComponent<DialogueParser>();
        Dialogue[] dialogues = theParser.Parse(csv_FileName);
        if (dialogues != null)
        {
            for (int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(dialogues[i].id, dialogues[i]);
            }
        }
    }

    public Dialogue[] GetDialogue(int _StartNum, int _EndNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        for (int i = _StartNum; i <= _EndNum; i++)
        {
            if (dialogueDic.TryGetValue(i, out Dialogue dialogue))
            {
                dialogueList.Add(dialogue);
            }
        }

        return dialogueList.ToArray();
    }
}