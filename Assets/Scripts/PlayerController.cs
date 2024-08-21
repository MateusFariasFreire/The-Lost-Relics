using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Camera mainCamera;

    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Interacting,
        Dashing,
        Defending,
        Attacking
    }

    public enum MovementDirection
    {
        Forward,
        Backward,
        Right,
        Left,
        None
    }

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.5f; // Durée du dash
    [SerializeField] private float attackDuration1 = 1f;
    [SerializeField] private float attackDuration2 = 1f;
    [SerializeField] private float attackDuration3 = 1f;
    [SerializeField] private float attackDuration4 = 1f;
    [SerializeField] private float interactionDuration = 2f;

    [Header("Attack Cooldowns")]
    [SerializeField] private float attackCooldown1 = 2f;
    [SerializeField] private float attackCooldown2 = 3f;
    [SerializeField] private float attackCooldown3 = 4f;
    [SerializeField] private float attackCooldown4 = 5f;

    [SerializeField] private PlayerState currentState = PlayerState.Idle;
    [SerializeField] private MovementDirection currentDirection = MovementDirection.None;
    [SerializeField] private MovementDirection relativeMovementDirection = MovementDirection.None;

    private Vector2 inputDirection;
    private bool isRunning;
    private bool isDashing;
    private bool isInteracting;
    private bool isDefending;
    private bool isAttacking;

    private float actionEndTime = 0f;
    private float attackEndTime = 0f;
    private float attackCooldownEndTime1 = 0f;
    private float attackCooldownEndTime2 = 0f;
    private float attackCooldownEndTime3 = 0f;
    private float attackCooldownEndTime4 = 0f;
    private float dashEndTime = 0f; // Ajouté pour suivre la fin du dash

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Time.time < actionEndTime)
        {
            // Bloque les actions spéciales si une animation est en cours
            if (isAttacking || isInteracting || isDashing)
            {
                return;
            }
        }

        if (isAttacking)
        {
            // Ne permet pas le mouvement pendant une attaque
            currentDirection = MovementDirection.None;
            return;
        }

        if (isDefending)
        {
            RotateTowardsMouse();
            return; // Ne fait rien d'autre pendant que le joueur défend
        }

        if (isInteracting || isDashing)
        {
            return; // Bloque les autres actions pendant interaction ou dash
        }

        // Gérer le mouvement et la direction après les contrôles d'état
        HandleMovement();
        RotateTowardsMouse();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            isRunning = true;
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
        }
    }

    public void OnDash(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            if (!isInteracting && !isAttacking && !isDefending && !isDashing)
            {
                StartCoroutine(PerformDash());
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            if (!isInteracting && !isAttacking && !isDashing && !isDefending)
            {
                StartCoroutine(HandleInteraction());
            }
        }
    }

    public void OnDefend(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            isDefending = true;
            currentState = PlayerState.Defending;
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            isDefending = false;
            currentState = PlayerState.Idle;
        }
    }

    public void OnAttack1(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed && Time.time > attackCooldownEndTime1)
        {
            StartCoroutine(PerformAttack(1));
        }
    }

    public void OnAttack2(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed && Time.time > attackCooldownEndTime2)
        {
            StartCoroutine(PerformAttack(2));
        }
    }

    public void OnAttack3(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed && Time.time > attackCooldownEndTime3)
        {
            StartCoroutine(PerformAttack(3));
        }
    }

    public void OnAttack4(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed && Time.time > attackCooldownEndTime4)
        {
            StartCoroutine(PerformAttack(4));
        }
    }

    private IEnumerator PerformAttack(int attackType)
    {
        if (isAttacking)
        {
            yield break; // Si déjà en train d'attaquer, ne fait rien
        }

        isAttacking = true;
        currentState = PlayerState.Attacking;

        // Définir la durée de l'attaque et la fin du cooldown en fonction du type d'attaque
        float attackDuration = 0f;
        float attackCooldown = 0f;

        switch (attackType)
        {
            case 1:
                attackDuration = attackDuration1;
                attackCooldown = attackCooldown1;
                break;
            case 2:
                attackDuration = attackDuration2;
                attackCooldown = attackCooldown2;
                break;
            case 3:
                attackDuration = attackDuration3;
                attackCooldown = attackCooldown3;
                break;
            case 4:
                attackDuration = attackDuration4;
                attackCooldown = attackCooldown4;
                break;
        }

        attackEndTime = Time.time + attackDuration;
        actionEndTime = attackEndTime; // Empêche les autres actions pendant l'attaque

        // Attendez la durée de l'attaque
        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        currentState = PlayerState.Idle;

        // Mettre à jour le temps de cooldown pour l'attaque effectuée
        switch (attackType)
        {
            case 1:
                attackCooldownEndTime1 = Time.time + attackCooldown;
                break;
            case 2:
                attackCooldownEndTime2 = Time.time + attackCooldown;
                break;
            case 3:
                attackCooldownEndTime3 = Time.time + attackCooldown;
                break;
            case 4:
                attackCooldownEndTime4 = Time.time + attackCooldown;
                break;
        }
    }

    private IEnumerator PerformDash()
    {
        if (isDashing || isAttacking || isInteracting || isDefending)
        {
            yield break; // Si déjà en train de dash ou d'une autre action, ne fait rien
        }

        isDashing = true;
        currentState = PlayerState.Dashing;

        Vector3 dashDirection = GetMovementDirection();
        float dashStartTime = Time.time;
        float dashEndTime = dashStartTime + dashDuration; // Définir la fin du dash en utilisant dashDuration

        while (Time.time < dashEndTime)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        currentState = PlayerState.Idle;
        actionEndTime = Time.time + dashDuration; // Bloquer les autres actions pendant la durée du dash
    }

    private IEnumerator HandleInteraction()
    {
        if (isInteracting || isAttacking || isDashing || isDefending)
        {
            yield break; // Si déjà en train d'interagir ou d'une autre action, ne fait rien
        }

        isInteracting = true;
        currentState = PlayerState.Interacting;
        actionEndTime = Time.time + interactionDuration;

        // Attendez la durée de l'interaction
        yield return new WaitForSeconds(interactionDuration);

        isInteracting = false;
        currentState = PlayerState.Idle;
    }

    private void HandleMovement()
    {
        if (inputDirection == Vector2.zero)
        {
            currentState = PlayerState.Idle;
            relativeMovementDirection = MovementDirection.None; // Aucune direction
            return;
        }

        Vector3 moveDirection = GetMovementDirection();
        float speed = isRunning ? runSpeed : walkSpeed;

        characterController.Move(moveDirection * speed * Time.deltaTime);
        currentDirection = DetermineMovementDirection(moveDirection);
        relativeMovementDirection = DetermineRelativeMovementDirection(moveDirection);
        currentState = isRunning ? PlayerState.Running : PlayerState.Walking;
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = GetCameraForward();
        Vector3 right = GetCameraRight();

        // Calculer la direction du mouvement en fonction des inputs du joueur
        Vector3 direction = forward * inputDirection.y + right * inputDirection.x;
        return direction.normalized; // Normaliser pour éviter des vitesses variables
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = mainCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private MovementDirection DetermineMovementDirection(Vector3 moveDirection)
    {
        Vector3 cameraForward = GetCameraForward();
        Vector3 cameraRight = GetCameraRight();

        // Calculer la direction relative du mouvement
        float dotForward = Vector3.Dot(cameraForward, moveDirection);
        float dotRight = Vector3.Dot(cameraRight, moveDirection);

        if (dotForward > 0.5f && Mathf.Abs(dotRight) < 0.5f)
        {
            return MovementDirection.Forward;
        }
        else if (dotForward < -0.5f && Mathf.Abs(dotRight) < 0.5f)
        {
            return MovementDirection.Backward;
        }
        else if (dotRight > 0.5f && Mathf.Abs(dotForward) < 0.5f)
        {
            return MovementDirection.Right;
        }
        else if (dotRight < -0.5f && Mathf.Abs(dotForward) < 0.5f)
        {
            return MovementDirection.Left;
        }

        return MovementDirection.None;
    }

    private MovementDirection DetermineRelativeMovementDirection(Vector3 moveDirection)
    {
        // Vous pouvez adapter cette méthode pour déterminer la direction relative souhaitée
        return currentDirection;
    }

    private void RotateTowardsMouse()
    {
        // Exemple de rotation vers la souris (ou autre point de ciblage)
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 directionToMouse = (hit.point - transform.position).normalized;
            directionToMouse.y = 0; // Ignorer la composante Y pour la rotation horizontale
            Quaternion targetRotation = Quaternion.LookRotation(directionToMouse);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
