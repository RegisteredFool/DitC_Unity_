using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NpcNameText;
    [SerializeField] private TextMeshProUGUI NpcDialogueText;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxTypeTime = 0.1f;
    [SerializeField] private GameObject currentChatter;

    private Queue<string> paragraphs = new Queue<string>();
    private bool conversationEnded;
    private string p;

    private Coroutine typeDialogue;
    private bool isTyping;

    private const string HTML_ALPHA = "<color=#00000000>";
    public void DisplayNextParagraph(NpcDialogue dialogueText)
    {
        if (paragraphs.Count == 0)
        {
            if (!conversationEnded)
            {
                StartConversation(dialogueText);
            }
            else if (conversationEnded && !isTyping)
            {
                EndConversation();
                return;
            }
        }
        //if something is in the queue
        //p = paragraphs.Dequeue();  this is for making text instant

        if (!isTyping)
        {
            p = paragraphs.Dequeue();
            typeDialogue = StartCoroutine(TypeDialogueText(p));
        }
        else
            FinishParagraphEarly();

            //update conversatoin text
            NpcDialogueText.text = p;

        if (paragraphs.Count == 0)
        {
            conversationEnded = true;
        }
    }

    void StartConversation(NpcDialogue dialogueText)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        NpcNameText.text = dialogueText.npcName;

        for (int i = 0; i < dialogueText.dialogueLines.Length; i++)
        {
            paragraphs.Enqueue(dialogueText.dialogueLines[i]);
        }
    }
    void EndConversation()
    {
        conversationEnded = false;
        currentChatter.GetComponent<DummyYea>().currentDialoguePath++;
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;
        NpcDialogueText.text = "";

        string originalText = p;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            NpcDialogueText.text = originalText;

            displayedText = NpcDialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            NpcDialogueText.text = displayedText;
            yield return new WaitForSeconds(maxTypeTime / speed);
        }
        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        StopCoroutine(typeDialogue);
        NpcDialogueText.text = p;
        isTyping = false;
    }
}
