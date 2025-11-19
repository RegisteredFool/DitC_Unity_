using UnityEngine;
using TMPro;

public class DummyYea : NPC, ITalk
{
    [SerializeField] private NpcDialogue dialogueText;
    //[SerializeField] private NpcDialogue npcName;
    [SerializeField] private DialogueController dialogueController;
    public int dialoguePaths;
    public int currentDialoguePath;
    public override void Interact()
    {
        Debug.Log("DummyYea");
        Talk(dialogueText);
    }

    public void Talk(NpcDialogue dialogueText)
    {
        dialogueController.DisplayNextParagraph(dialogueText);
    }
}
