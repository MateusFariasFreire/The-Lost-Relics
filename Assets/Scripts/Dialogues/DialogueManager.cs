using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string>sentences;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;

    private DialogueTrigger lastDialogTrigger;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    //make this class a singleton
    public static DialogueManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DialogueManager found!");
            return;
        }
        instance = this;
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger dialogueTrigger)
    {
        lastDialogTrigger = dialogueTrigger;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sentences.Clear();
        GameManager.Instance.SetDialogueActive(true);
        canvas.enabled = true;


        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        nameText.text = dialogue.name;
        animator.SetBool("IsOpen", true);
        //focus sur le bouton
        continueButton.Select();

        DisplayNextSentence();
    }

    

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        StopAllCoroutines();
        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        GameManager.Instance.SetDialogueActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        animator.SetBool("IsOpen", false);
        if (lastDialogTrigger != null)
        {
            lastDialogTrigger.SetDialogueAsEnded();
        }

        canvas.enabled = false;
        lastDialogTrigger = null;
    }
}
