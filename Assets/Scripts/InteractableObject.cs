using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Canvas interactCanvas;

    private void Start()
    {
        interactCanvas.enabled = false;
    }

    public void OnInteract()
    {
       transform.parent.SendMessage("Interact");
    }

    public void DisplayCanvas()
    {
        interactCanvas.enabled = true;
    }

    public void HideCanvas()
    {
        interactCanvas.enabled = false;
    }
}
