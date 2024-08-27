using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MessageDisplay : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Référence au composant Text UI
    public float displayDuration = 5f; // Durée d'affichage du message en secondes

    private void Start()
    {
        if (messageText == null)
        {
            Debug.LogError("MessageText component is not assigned.");
        }
        ShowMessage("Test de la zone de text");
    }

    public void ShowMessage(string message)
    {
        StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        messageText.gameObject.SetActive(false);
    }
}