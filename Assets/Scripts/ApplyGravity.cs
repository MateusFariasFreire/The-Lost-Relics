using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravity : MonoBehaviour
{
    [SerializeField] private float gravity = 9.8f; // Force de gravité personnalisée
    [SerializeField] private LayerMask groundLayer; // Layer du sol
    [SerializeField] private float rayDistance = 1.1f; // Distance du Raycast (ajustez selon la hauteur du joueur)

    private bool isGrounded = false;

    void Update()
    {
        // Lancer un Raycast vers le bas pour vérifier si le joueur est au sol
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundLayer);

        // Si le joueur n'est pas au sol, appliquez la gravité
        if (!isGrounded)
        {
            // Appliquer la gravité en déplaçant le joueur vers le bas
            transform.position += Vector3.down * gravity * Time.deltaTime;
        }
        else
        {
            // Optionnel : Ajuster la position du joueur pour s'assurer qu'il est juste au-dessus du sol
            float groundHeight = hit.point.y;
            float playerHeight = transform.localScale.y / 2; // Supposant que le centre du joueur est au milieu du collider
            transform.position = new Vector3(transform.position.x, groundHeight + playerHeight, transform.position.z);
        }
    }

    private void OnDrawGizmos()
    {
        // Dessiner un rayon rouge pour visualiser le Raycast dans l'éditeur
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
    }
}
