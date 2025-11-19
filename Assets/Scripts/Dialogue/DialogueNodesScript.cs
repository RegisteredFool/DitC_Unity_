using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;
using System.ComponentModel;
using UnityEditor;
using Unity.Properties;

[Serializable]

public class StartNode : Node 
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort("out").Build();
    }
}
    [Serializable]
public class EndNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
        context.AddInputPort<int>("NextDialogue").Build();
    }
}
public class DialogueNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
        context.AddOutputPort("out").Build();

        context.AddInputPort<string>("Speaker").Build();
        context.AddInputPort<string>("Dialogue").Build();
        context.AddInputPort<Sprite>("Face").Build();
        context.AddInputPort<bool>("Animate").Build();
        context.AddInputPort<int>("NextDialogue").Build();
    }
}

[Serializable]
public class ChoiceNode : Node
{
    const string optionID = "portCount";
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();

        context.AddInputPort<string>("Speaker").Build();
        context.AddInputPort<string>("Dialogue").Build();
        context.AddInputPort<Sprite>("Face").Build();
        context.AddInputPort<int>("NextDialogue").Build();
        var option = GetNodeOptionByName(optionID);
        option.TryGetValue(out int portCount);
        for (int i = 0; i < portCount; i++)
        {
            context.AddInputPort<string>($"Choice Text{i}").Build();
            context.AddOutputPort($"Choice {i}").Build();
        }
    }

    protected override void OnDefineOptions(INodeOptionDefinition context)
    {
        //context.AddNodeOption<int>(optionID).WithDefaultValue(2).Delayed();//,
        context.AddNodeOption<int>(optionID);    //, defaultValue: 2, new Attribute[] {new DelayedAttribute()});
    }
}
