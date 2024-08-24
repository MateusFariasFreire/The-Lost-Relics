using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Walking,
        Attacking
    }

    public enum AttackType
    {
        Melee,
        Ranged
    }

    [SerializeField] private EnemyState currentState = EnemyState.Idle;

    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    [Header("Fighting Stats")]
    [SerializeField] private int damage = 10;
    [SerializeField] private int health = 50;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float attackAnimationDuration = 1f;
    [SerializeField] private AttackType attackType = AttackType.Melee;
    public bool canAttack = true;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float maxVerticalDifference = 2f;
    [SerializeField] private LayerMask environmentLayerMask;
    
    private GameObject player;
    private Animator animator;
    private CharacterController characterController;

    private bool isGrounded = false;
    private bool isAttacking = false;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking)
        {
            ApplyGravity();
            return;
        }

        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                if (canAttack)
                {
                    SetState(EnemyState.Attacking);
                    Attack();
                }
                else
                {
                    SetState(EnemyState.Idle);
                }
            }
            else
            {
                if (isGrounded)
                {
                    SetState(EnemyState.Walking);
                    RotateAndMoveTowardsPlayer();
                }
            }
        }
        else
        {
            checkForPlayer();
            SetState(EnemyState.Idle);
        }

        ApplyGravity();
        UpdateAnimations();
    }

    private void RotateAndMoveTowardsPlayer()
    {
        if (isAttacking) return;

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = targetRotation;

        Vector3 moveDirection = transform.forward * moveSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void checkForPlayer()
    {
        Vector3 overlapPos = transform.position;
        overlapPos.y += 1f;

        Collider[] hitColliders = Physics.OverlapSphere(overlapPos, detectionRange);

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
        StartCoroutine(PerformAttack());
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        canAttack = false;

        animator.Play("Attack");
        yield return new WaitForSeconds(attackAnimationDuration);

        if (attackType == AttackType.Melee)
        {
            player.GetComponent<HealthAndManaManager>().TakeDamage(damage);
        }

        isAttacking = false;

        yield return new WaitForSeconds(cooldown - attackAnimationDuration);

        canAttack = true;

    }

    private void ApplyGravity()
    {
        isGrounded = characterController.isGrounded;

        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -2f;
        }

        Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
        characterController.Move(gravityMove * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        switch (currentState)
        {
            case EnemyState.Walking:
                animator.Play("Walking");
                break;
            case EnemyState.Idle:
                animator.Play("Idle");
                break;
        }
    }

    private void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    private IEnumerator ResetAttackStateAfterAnimation()
    {
        yield return new WaitForSeconds(attackAnimationDuration);
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 overlapPos = transform.position;
        overlapPos.y += 1f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(overlapPos, attackRange);

        DrawDetectionCylinder(transform.position, detectionRange, maxVerticalDifference);
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
