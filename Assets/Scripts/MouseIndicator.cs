using UnityEngine;
using UnityEngine.InputSystem;

public class MouseIndicator : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        gameObject.GetComponent<MeshRenderer>().enabled = false; // Désactiver l'indicateur au début
    }

    private void Update()
    {
        UpdateIndicatorPosition();
    }

    private void UpdateIndicatorPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Mettre à jour la position du GameObject auquel le script est attaché
            transform.position = hit.point;
            gameObject.GetComponent<MeshRenderer>().enabled = true; // Activer l'indicateur lorsqu'il est sur une surface
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;  // Désactiver l'indicateur si pas de surface touchée
        }
    }
}
