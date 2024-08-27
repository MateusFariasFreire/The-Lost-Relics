using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;

    private bool dialogueFinished = false;

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue, this);
    }

    public void SetDialogueAsEnded()
    {
        dialogueFinished = true;
        this.SendMessage("OnDialogueEnd", SendMessageOptions.DontRequireReceiver);
    }
}
