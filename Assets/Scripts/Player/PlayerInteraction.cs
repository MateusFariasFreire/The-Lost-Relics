using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    InteractableObject lastIntaractableObject = null;

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject;
        if (other.TryGetComponent<InteractableObject>(out interactableObject))
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject interactableObject;
        if (other.TryGetComponent<InteractableObject>(out interactableObject))
        {
            if (lastIntaractableObject == interactableObject)
            {
                lastIntaractableObject.HideCanvas();
                lastIntaractableObject = null;
                return;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        InteractableObject interactableObject;
        if (other.TryGetComponent<InteractableObject>(out interactableObject))
        {
            if (lastIntaractableObject == interactableObject)
            {
                return;
            }

            if(lastIntaractableObject == null)
            {
                lastIntaractableObject = interactableObject;
                lastIntaractableObject.DisplayCanvas();
            }
            else
            {

                float distanceToLast = Vector3.Distance(transform.position, lastIntaractableObject.transform.position);
                float distanceToCurrent = Vector3.Distance(transform.position, interactableObject.transform.position);

                if (distanceToCurrent < distanceToLast)
                {
                    lastIntaractableObject.HideCanvas();
                    lastIntaractableObject = interactableObject;
                    lastIntaractableObject.DisplayCanvas();
                }
            }
        }
    }

    public void Interact()
    {
        if (lastIntaractableObject != null)
        {
            lastIntaractableObject.SendMessage("OnInteract");
        }
    }
}
