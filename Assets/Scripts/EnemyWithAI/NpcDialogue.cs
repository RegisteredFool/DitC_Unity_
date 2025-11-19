using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]

public class NpcDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;

    [TextArea(5,10)]
    public string[] dialogueLines;

    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1.0f;
    public bool[] autoProgress;
    public float autoProgressDelay = 1.5f;
}
