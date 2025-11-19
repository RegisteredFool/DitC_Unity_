using System;
using UnityEngine;
using System.Collections.Generic;
public class RuntimeDialogueGraph : ScriptableObject
{
    public string EntryNodeID;
    public List<RuntimeDialogueNode> AllNodes = new List<RuntimeDialogueNode>();
}

[Serializable]
public class RuntimeDialogueNode
{
    public string NodeID;
    public string SpeakerName;
    public string DialogueText;
    public Sprite SpeakerFace;
    public bool anim;
    public List<ChoiceData> Choices = new List<ChoiceData>();
    public string NextNodeID;
    public int NewDialogue;
}

[Serializable]
public class ChoiceData
{
    public string ChoiceText;
    public string DestinationNodeID;
}
