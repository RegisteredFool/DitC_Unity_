using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject player;
    public RuntimeDialogueGraph[] RuntimeGraph;
    [SerializeField] private int dialogueIndex;
    [SerializeField] private float offset;
    [SerializeField] private float timeOffset;

    [Header("UI Components")]
    public GameObject DialoguePanel; //whole thing
    public GameObject dialogueHolder;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Image npcImage;

    [Header("Choice Button UI")]
    public Button choiceButtonPrefab;
    public Transform choiceButtonContainer;

    [Header("Sound")]
    public AudioClip typeSound;
    public AudioClip fullSound;

    private Dictionary<string, RuntimeDialogueNode> nodeLookup = new Dictionary<string, RuntimeDialogueNode>();
    private RuntimeDialogueNode currentNode;

    //Type dialogue letter-by-letter
    private Coroutine typeDialogueCorutine;
    private bool isTyping;
    [SerializeField] float typeSpeed = .05f;
    private const string HTML_ALPHA = "<color=#00000000>";
    private string p;

    private void Start()
    {
        if (DialoguePanel.activeSelf)
            DialoguePanel.SetActive(false);
    }
    public void BeginDialogue()
    {
        if (!DialoguePanel.activeSelf)
            DialoguePanel.SetActive(true);
        else
        {
            if (!string.IsNullOrEmpty(currentNode.NextNodeID) && !isTyping)
            {   
                ShowNode(currentNode.NextNodeID);
            }
            else if (!string.IsNullOrEmpty(currentNode.NextNodeID) && isTyping)
            {
                FinishParagraphEarly();
            }
            else if (!string.IsNullOrEmpty(currentNode.NextNodeID))
            {
                EndDialogue();
            }
            return;
        }
        //Time.timeScale = 0;

        dialogueHolder.GetComponent<RectTransform>().position = new Vector2(dialogueHolder.GetComponent<RectTransform>().position.x, offset);
        LeanTween.moveY(dialogueHolder.GetComponent<RectTransform>(), -359, timeOffset).setEaseOutQuad();
        //dialogueText.GetComponent<RectTransform>().position = new Vector2(dialogueText.GetComponent<RectTransform>().position.x, offset);
        //LeanTween.moveY(dialogueText.GetComponent<RectTransform>(), -299  , timeOffset); 

        foreach (var node in RuntimeGraph[dialogueIndex].AllNodes)
        {
            nodeLookup[node.NodeID] = node;
        }
        if (!string.IsNullOrEmpty(RuntimeGraph[dialogueIndex].EntryNodeID))
        {
            ShowNode(RuntimeGraph[dialogueIndex].EntryNodeID);
            //Time.timeScale = 0;
            player.GetComponent<PlayerInput>().DeactivateInput();
            Debug.Log("wow");
        }
        else
        {
            EndDialogue();
        }
    }

    private void Update()
    {
        if ((Mouse.current.leftButton.wasPressedThisFrame || Input.GetKeyDown("e")) && currentNode != null && currentNode.Choices.Count == 0)
        {
            if (!string.IsNullOrEmpty(currentNode.NextNodeID) && !isTyping)
            {
                ShowNode(currentNode.NextNodeID);
            }
            else if (!string.IsNullOrEmpty(currentNode.NextNodeID) && isTyping)
            {
                FinishParagraphEarly();
            }
            else if (!string.IsNullOrEmpty(currentNode.NextNodeID))
            {
                EndDialogue();
            }
        }
    }
    private void ShowNode(string nodeID)
    {
        if (!nodeLookup.ContainsKey(nodeID))
        {
            EndDialogue();
            return;
        }
        currentNode = nodeLookup[nodeID];
        speakerNameText.SetText(currentNode.SpeakerName);
        npcImage.GetComponent<Image>().sprite = currentNode.SpeakerFace;
        if (fullSound != null)
            SFXManager.instance.PlaySound(fullSound, transform.position, 1f);
        if (currentNode.anim == true)
            npcImage.GetComponent<Animator>().SetBool("ActiveDamage",true);
        if (!isTyping)
        {
            p = currentNode.DialogueText;
            typeDialogueCorutine = StartCoroutine(TypeDialogueText(p));
        }

        foreach (Transform child in choiceButtonContainer) //destroy any buttons that might currently exist
        {
            Destroy(child.gameObject);
        }
        if (currentNode.Choices.Count > 0)
        {
            foreach (var choice in currentNode.Choices)
            {
                Button button = Instantiate(choiceButtonPrefab, choiceButtonContainer);
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                //npcImage.GetComponent<Image>().sprite = currentNode.SpeakerFace;
                if (buttonText != null)
                {
                    buttonText.text = choice.ChoiceText;
                }
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        if (!string.IsNullOrEmpty(choice.DestinationNodeID))
                        {
                            if (isTyping)
                                FinishParagraphEarly();
                            ShowNode(choice.DestinationNodeID);
                        }
                        else
                        {
                            EndDialogue();
                        }
                    });
                }
            }
        }

        
    }
    private void fin()
    {
        DialoguePanel.SetActive(false);
    }
    private void EndDialogue()
    {
        dialogueIndex = currentNode.NewDialogue;
        //dialogueHolder.GetComponent<RectTransform>().position = new Vector2(dialogueHolder.GetComponent<RectTransform>().position.x, offset);
        LeanTween.moveY(dialogueHolder.GetComponent<RectTransform>(), offset*6f, timeOffset).setEaseOutQuad().setOnComplete(fin);
        
        currentNode = null;

        foreach (Transform child in choiceButtonContainer)
        {
            Destroy(child.gameObject);
        }
        Time.timeScale = 1.0f;
        player.GetComponent<PlayerInput>().ActivateInput();

    }

    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        dialogueText.text = "";
        string originalText = p;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            dialogueText.text = originalText;
            displayedText = dialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            dialogueText.text = displayedText;
            if (typeSound != null)
                SFXManager.instance.PlaySound(typeSound, transform.position, 1f);
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        isTyping = false;
    }
    private void FinishParagraphEarly()
    {
        StopCoroutine(typeDialogueCorutine);

        dialogueText.text = p;

        isTyping = false;
    }
}
