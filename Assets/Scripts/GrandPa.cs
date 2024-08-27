using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandPa : MonoBehaviour
{

    [SerializeField] private ScenarioManager scenarioManager;
    [SerializeField] DialogueTrigger firstDialog;
    [SerializeField] DialogueTrigger foundBookDialog;
    [SerializeField] DialogueTrigger defeatedBoss;

    public void Interact()
    {
        if (scenarioManager.hasDefeatedBoss)
        {
            defeatedBoss.TriggerDialogue();
        }
        else if (scenarioManager.hasFoundBook)
        {
            foundBookDialog.TriggerDialogue();
        }
        else
        {
            firstDialog.TriggerDialogue();
        }
    }

    public void OnDialogueEnd()
    {
    }
}
