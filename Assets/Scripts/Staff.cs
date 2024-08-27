using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private ScenarioManager scenarioManager;

    public void Interact()
    {
        DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();
        dialogueTrigger.TriggerDialogue();
    }

    public void OnDialogueEnd()
    {
        scenarioManager.OnWandFound();
        scenarioManager.OnStaffFound();
        Destroy(gameObject);
    }
}
