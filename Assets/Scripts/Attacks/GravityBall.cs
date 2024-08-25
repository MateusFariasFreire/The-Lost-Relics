using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBall : MonoBehaviour
{
    [SerializeField] private float attractionForce = 10f;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float groundDistance = 3f;
    [SerializeField] private float pullDistance = 5f;
    private float applyDamageTimer = 0f;

    private int _damage = 0;
    private void Update()
    {
        FollowMouse();

        applyDamageTimer += Time.deltaTime;
        if (applyDamageTimer >= 1f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, pullDistance, LayerMask.GetMask("Enemies"));
            foreach (Collider collider in colliders)
            {
                collider.SendMessage("TakeDamage", _damage);
            }

            applyDamageTimer = 0f;
        }
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

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pullDistance);
    }
}
