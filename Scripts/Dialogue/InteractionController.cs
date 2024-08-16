using System.Collections;
using UnityEngine;

public class InteractionController : InteractiveObject
{
    [SerializeField] private int startDialogueIndex;
    [SerializeField] private int endDialogueIndex;
    private Dialogue[] dialogues;
    private DialogueState dialogueState = DialogueState.Inactive;
    private bool canStartDialogue = true;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            //Debug.LogError("PlayerController not found in the scene.");
        }
    }
    protected override void OnInteraction()
    {
        if (dialogueState != DialogueState.Inactive || !canStartDialogue)
            return;

        base.OnInteraction();
        dialogues = DatabaseManager.Instance.GetDialogue(startDialogueIndex, endDialogueIndex);

        if (dialogues != null && dialogues.Length > 0)
        {
            if (playerController != null)
            {
                dialogueState = DialogueState.DialogueStarted;
                playerController.DeceleratePlayer();
                StartCoroutine(StartDialogueAfterDeceleration(playerController));
            }
            
        }
    }
    private IEnumerator StartDialogueAfterDeceleration(PlayerController player)
    {
        yield return new WaitUntil(() => player.GetComponent<Rigidbody2D>().velocity.magnitude == 0);
        DialogueManager.Instance.ShowDialogue(dialogues, EndInteraction);
    }
    private void EndInteraction()
    {
        DialogueManager.Instance.ResetDialogueState();
        dialogueState = DialogueState.Inactive;
        canStartDialogue = true;
    }

    private void Update()
    {
        if (isInteractiveObject && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueState == DialogueState.DialogueStarted)
            {
                if (DialogueManager.Instance.IsDialogueActive())
                {
                    DialogueManager.Instance.DisplayNextLine();
                    //Debug.Log("InteractiveObject대화");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player"))
        {
            if (objectType == ObjectType.AutoDialogue)
            {
                if (playerController != null && dialogueState == DialogueState.Inactive && canStartDialogue)
                {
                    playerController.StopPlayer();
                    OnInteraction();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueState = DialogueState.Inactive;
            if(objectType == ObjectType.AutoDialogue)
            {
                playerController.ResumePlayer();
                canStartDialogue = false;
            }
            
        }
    }
}