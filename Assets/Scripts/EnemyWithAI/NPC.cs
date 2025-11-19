using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer interactSprite;

    private Transform playerTransform;
    [SerializeField] private float interactRange;
    private bool inInteractRange;

    public abstract void Interact();

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E");
            Interact();
        }

        if (WithinInteractionRange() && !interactSprite.gameObject.activeSelf)
        {
            interactSprite.gameObject.SetActive(true);
        }
        else if (!WithinInteractionRange() && interactSprite.gameObject.activeSelf)
        {
            interactSprite.gameObject.SetActive(false);
        }
    }

    private bool WithinInteractionRange()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) < interactRange)
                return true;
        else
            return false;
    }


}









/*
public NpcDialogue dialogueData;
public GameObject dialoguePanel;
public TMP_Text dialogueText, nameText;
public Image portraitImage;
public Pause pauseManager;
public GameObject dialogueCanvas;

[SerializeField] private int dialogueIndex;
[SerializeField] private bool isTyping, isDialogueActive;

private const string HTML_ALPHA = "<color=#00000000>";

public bool CanIntact()
{
    return !isDialogueActive;
}
public void Interact()
{
    Debug.Log("Did a thing");
    if (dialogueData == null)// || (pauseManager.pause == true && !isDialogueActive))
        return;
    if (isDialogueActive)
    {
        NextLine();
    }
    else
    {
        dialogueCanvas.SetActive(true);
        StartDialogue();
    }
}

/* public void StartDialogue()
{
    isDialogueActive = true;
    dialogueIndex = 0;

    nameText.SetText(dialogueData.name);
    portraitImage.sprite = dialogueData.npcPortrait;

    dialoguePanel.SetActive(true);
    //pauseManager.AlternatePause();

    StartCoroutine(TypeLine());
}
public void StartDialogue(string p)
{
    isTyping = true;




    isTyping = false;
}
void NextLine()
{
    if (isTyping)
    {
        StopAllCoroutines();
        dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
        isTyping = false;
    }
    else if (++dialogueIndex < dialogueData.dialogueLines.Length)
    {
        StartCoroutine(TypeLine());
    }
    else
    {
        EndDialogue();
    }
}

IEnumerator TypeLine()
{
    isTyping = true;
    dialogueText.SetText("");

    foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
    {
        dialogueText.text += letter;
        yield return new WaitForSeconds(dialogueData.typingSpeed);
    }
    isTyping = false;

    if (dialogueData.autoProgress.Length > dialogueIndex && dialogueData.autoProgress[dialogueIndex])
    {
        yield return new WaitForSeconds(dialogueData.autoProgressDelay);
        NextLine();
    }
}
public void EndDialogue()
{
    StopAllCoroutines();
    dialogueCanvas.SetActive(false);
    isDialogueActive = false;
    dialogueText.SetText("");
    dialoguePanel.SetActive(false);

    //pauseManager.AlternatePause();
}

} */
