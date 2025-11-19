using UnityEngine;
using UnityEditor.AssetImporters;
using Unity.GraphToolkit.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;

[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueGraphImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath); //loads editor graph, then populate
        RuntimeDialogueGraph runtimeGraph = ScriptableObject.CreateInstance<RuntimeDialogueGraph>();
        var nodeIDMap = new Dictionary<INode, string>();

        foreach (var node in editorGraph.GetNodes())
        {
            nodeIDMap[node] = Guid.NewGuid().ToString(); //access each node id
        }

        var startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();
        if (startNode != null)
        {
            var entryPort = startNode.GetOutputPorts().FirstOrDefault().firstConnectedPort;
            if (entryPort != null)
            {
                runtimeGraph.EntryNodeID = nodeIDMap[entryPort.GetNode()];
            }
        }

        foreach (var iNode in editorGraph.GetNodes())
        {
            if (iNode is StartNode || iNode is EndNode) continue; //skips the node if start or end

            var runtimeNode = new RuntimeDialogueNode { NodeID = nodeIDMap[iNode] }; //grab id from dictionary, be it choice or dialogue
            if (iNode is DialogueNode dialogueNode)
            {
                ProcessDialogueNode(dialogueNode, runtimeNode, nodeIDMap);
            }
            else if (iNode is ChoiceNode choiceNode)
            {
                ProcessChoiceNode(choiceNode, runtimeNode, nodeIDMap);
            }
            else if (iNode is EndNode endNode)
            {
                ProcessEndDialogueNode(endNode, runtimeNode, nodeIDMap);
            }
            runtimeGraph.AllNodes.Add(runtimeNode);
        }

        ctx.AddObjectToAsset("RuntimeData", runtimeGraph);
        ctx.SetMainObject(runtimeGraph);
    }
    private void ProcessDialogueNode(DialogueNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue"));
        runtimeNode.SpeakerFace = GetPortValue<Sprite>(node.GetInputPortByName("Face"));
        runtimeNode.NewDialogue = GetPortValue<int>(node.GetInputPortByName("NextDialogue"));
        runtimeNode.anim = GetPortValue<bool>(node.GetInputPortByName("Animate"));

        var nextNodePort = node.GetOutputPortByName("out")?.firstConnectedPort; 
        if (nextNodePort != null)
            runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()]; //makes the chain of going through each of the nodes
    }
    private void ProcessChoiceNode(ChoiceNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue"));
        runtimeNode.SpeakerFace = GetPortValue<Sprite>(node.GetInputPortByName("Face"));
        runtimeNode.NewDialogue = GetPortValue<int>(node.GetInputPortByName("NextDialogue"));

        var choiceOutputPorts = node.GetOutputPorts().Where(p => p.name.StartsWith("Choice ")); //gets the ports where the name starts with choice

        foreach(var outputPort in choiceOutputPorts)
        {
            var index = outputPort.name.Substring("Choice ".Length);
            var textPort = node.GetInputPortByName($"Choice Text{index}"); //get the index of each port, and get them by their full name

            var choiceData = new ChoiceData //populate the data of the ports
            {
                ChoiceText = GetPortValue<string>(textPort),
                DestinationNodeID = outputPort.firstConnectedPort != null ? nodeIDMap[outputPort.firstConnectedPort.GetNode()] : null
            };
            runtimeNode.Choices.Add(choiceData);
        }
    }
    private void ProcessEndDialogueNode(EndNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.NewDialogue = GetPortValue<int>(node.GetInputPortByName("NextDialogue"));
    }
    private T GetPortValue<T>(IPort port)
    {
        if (port == null) return default; //if there is nothing connected, use default value (if int, use 0, if string, use ""

        if (port.isConnected)
        {
            if (port.firstConnectedPort.GetNode() is IVariableNode variableNode) //if something is plugged into the node, use it
            {
                variableNode.variable.TryGetDefaultValue(out T value);
                return value;
            }
        }
        port.TryGetValue(out T fallbackValue); //if port isn't connected, BUT there is a filled in value, use this
        return fallbackValue;
    }
}
