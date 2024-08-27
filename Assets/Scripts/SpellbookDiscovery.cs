using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellbookDiscovery : MonoBehaviour
{
    [SerializeField] private ScenarioManager scenarioManager;

    public void Interact()
    {
        DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();
        dialogueTrigger.TriggerDialogue();
    }

    public void OnDialogueEnd()
    {
        scenarioManager.OnBookFound();
        Destroy(gameObject);
    }
}
