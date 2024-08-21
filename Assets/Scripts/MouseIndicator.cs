using UnityEngine;
using UnityEngine.InputSystem;

public class MouseIndicator : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        gameObject.GetComponent<MeshRenderer>().enabled = false; // D�sactiver l'indicateur au d�but
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
            // Mettre � jour la position du GameObject auquel le script est attach�
            transform.position = hit.point;
            gameObject.GetComponent<MeshRenderer>().enabled = true; // Activer l'indicateur lorsqu'il est sur une surface
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;  // D�sactiver l'indicateur si pas de surface touch�e
        }
    }
}
