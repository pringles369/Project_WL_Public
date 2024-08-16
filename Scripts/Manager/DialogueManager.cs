using System;
using UnityEngine;
using TMPro;

public class DialogueManager : GenericSingleton<DialogueManager>
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject dialogueNameBar;
    //[SerializeField] Image portraitImage;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI dialogueNameText;

    Dialogue[] dialogues;
    int lineCount = 0;
    private bool isDialogue = false;
    private Action onDialogueEnd;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);  // 씬 전환 시 객체가 파괴되지 않도록 설정
        dialoguePanel.SetActive(false);
        dialogueNameBar.SetActive(false);
        //portraitImage.gameObject.SetActive(false);
    }

    public void ResetDialogueState()
    {
        dialogues = null;
        lineCount = 0;
        isDialogue = false;
        onDialogueEnd = null;
        SettingUI(false);
    }

    public void ShowDialogue(Dialogue[] p_dialogues, Action onEndCallback = null)
    {
        ResetDialogueState();
        dialogues = p_dialogues;
        lineCount = 0;
        onDialogueEnd = onEndCallback;
        StopAllEnemies();
        StopPlayer();
        SettingUI(true);
        DisplayNextLine();
        isDialogue = true;

    }

    public void DisplayNextLine()
    {
        if(lineCount < dialogues.Length)
        {
            dialogueNameText.text = dialogues[lineCount].name;
            dialogueText.text = string.Join("\n", dialogues[lineCount].contexts);
            lineCount++;
        }
        else
        {
            ResumeAllEnemies();
            ResumePlayer();
            SettingUI(false);
            isDialogue = false;
            onDialogueEnd?.Invoke();
        }
    }

    public void SettingUI(bool p_flag)
    {
        dialoguePanel.SetActive(p_flag);
        dialogueNameBar.SetActive(p_flag);
    }

    public bool IsDialogueActive()
    {
        return isDialogue;
    }

    private void StopAllEnemies()
    {
        EnemyMove[] enemies = FindObjectsOfType<EnemyMove>();
        foreach (var enemy in enemies)
        {
            enemy.StopEnemyMovement();
        }
    }

    private void ResumeAllEnemies()
    {
        EnemyMove[] enemies = FindObjectsOfType<EnemyMove>();
        foreach (var enemy in enemies)
        {
            enemy.ResumeEnemyMovement();
        }
    }

    private void StopPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.StopPlayer();
        }
    }

    private void ResumePlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.ResumePlayer();
        }
    }
}