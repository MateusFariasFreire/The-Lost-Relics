using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float maxVerticalDifference = 2f;

    [SerializeField] private LayerMask environmentLayerMask;

    private Animator animator;

    CharacterController characterController;

    private float verticalVelocity = 0f;
    [SerializeField] private float gravity = -9.81f;

    bool isGrounded = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        checkForPlayer();

        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                Attack();
            }
            else
            {
                if (isGrounded)
                {
                    RotateAndMoveTowardsPlayer();
                }
            }
        }

        ApplyGravity();
    }

    private void RotateAndMoveTowardsPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Appliquer directement la rotation cible sur l'axe Y
        transform.rotation = targetRotation;

        // Utiliser CharacterController pour le mouvement
        Vector3 moveDirection = transform.forward * moveSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void checkForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Player"))
            {
                Vector3 playerPosition = hitCollider.transform.position;

                if (Mathf.Abs(transform.position.y - playerPosition.y) <= maxVerticalDifference)
                {
                    Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
                    float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

                    if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, environmentLayerMask))
                    {
                        player = hitCollider.gameObject;
                        return;
                    }
                }
            }
        }

        player = null;
    }

    private void Attack()
    {
        Debug.Log("Attacked");
    }

    private void ApplyGravity()
    {
        isGrounded = characterController.isGrounded;

        if (!isGrounded)
        {
            // Appliquer la gravité si l'ennemi n'est pas au sol
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -2f; // Assurer que l'ennemi reste au sol
        }

        Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
        characterController.Move(gravityMove * Time.deltaTime);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        DrawDetectionCylinder(transform.position, detectionRange, maxVerticalDifference);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void DrawDetectionCylinder(Vector3 center, float radius, float height)
    {
        int segments = 36;
        float angleStep = 360f / segments;
        float halfHeight = height / 2;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float nextAngle = (i + 1) * angleStep * Mathf.Deg2Rad;

            Vector3 point1 = center + new Vector3(Mathf.Cos(angle) * radius, -halfHeight, Mathf.Sin(angle) * radius);
            Vector3 point2 = center + new Vector3(Mathf.Cos(nextAngle) * radius, -halfHeight, Mathf.Sin(nextAngle) * radius);
            Vector3 point3 = center + new Vector3(Mathf.Cos(angle) * radius, halfHeight, Mathf.Sin(angle) * radius);
            Vector3 point4 = center + new Vector3(Mathf.Cos(nextAngle) * radius, halfHeight, Mathf.Sin(nextAngle) * radius);

            Gizmos.DrawLine(point1, point2);
            Gizmos.DrawLine(point3, point4);
            Gizmos.DrawLine(point1, point3);
            Gizmos.DrawLine(point2, point4);
        }
    }
}
