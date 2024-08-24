using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBall : MonoBehaviour
{
    [SerializeField] private float attractionForce = 10f;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float groundDistance = 3f;
    [SerializeField] private float pullDistance = 5f; // Distance à laquelle l'attraction commence

    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        Vector3 mousePos = MouseIndicator.GetMouseWorldPosition();
        mousePos.y += groundDistance;
        transform.position = Vector3.Lerp(transform.position, mousePos, lerpSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        CharacterController character = other.GetComponent<CharacterController>();

        if (character != null)
        {
            Vector3 direction = transform.position - other.transform.position;
            direction.y = 0;

            // Appliquer une force d'attraction uniforme
            Vector3 pull = direction.normalized * attractionForce * Time.deltaTime;

            // Déplacer le CharacterController directement
            character.Move(pull);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pullDistance);
    }
}
