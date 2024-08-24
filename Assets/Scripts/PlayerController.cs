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
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float interactionDuration = 2f;

    [Header("Dash Effect")]
    [SerializeField] private MeshTrail dashMeshTrail;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    public PlayerState CurrentState { get { return currentState; } }
    [SerializeField] private PlayerState currentState = PlayerState.Idle;
    private PlayerState oldState = PlayerState.Idle;
    private MovementDirection currentDirection = MovementDirection.None;
    private MovementDirection relativeMovementDirection = MovementDirection.None;

    private Vector2 inputDirection;
    private bool isRunning;
    private bool isDashing;
    private bool isInteracting;
    private bool isAttacking;
    private bool isGrounded;

    private float actionEndTime = 0f;

    private Animator playerAnimator;
    private PlayerAttacks playerAttacks;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        playerAttacks = GetComponent<PlayerAttacks>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Time.time < actionEndTime)
        {
            if (isAttacking || isInteracting || isDashing)
            {
                return;
            }
        }

        if (isAttacking)
        {
            currentDirection = MovementDirection.None;
            return;
        }

        if (isInteracting || isDashing)
        {
            return;
        }

        // Gérer le mouvement et la rotation
        HandleMovement();
        RotateTowardsMouse();

        // Appliquer la gravité
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (!isDashing)
        {
            isGrounded = characterController.isGrounded;

            if (isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f; // Garder une légère pression vers le sol pour éviter de quitter le sol
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime; // Appliquer la gravité en continu
            }

            Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
            characterController.Move(gravityMove * Time.deltaTime);
        }
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
            if (!isInteracting && !isAttacking && !isDashing)
            {
                StartCoroutine(PerformDash());
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            if (!isInteracting && !isAttacking && !isDashing)
            {
                StartCoroutine(HandleInteraction());
            }
        }
    }

    public void OnAttack1(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            StartCoroutine(HandleAttack(1));
        }
    }

    public void OnAttack2(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            StartCharging(2);
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            Attack(2);
        }
    }

    public void OnAttack3(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            StartCharging(3);
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            Attack(3);
        }
    }

    public void OnAttack4(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            StartCharging(4);
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            Attack(4);
        }
    }

    public void OnAttack5(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            StartCoroutine(HandleAttack(5));
        }
    }

    private IEnumerator PerformDash()
    {
        if (isDashing || isAttacking || isInteracting)
        {
            yield break;
        }

        isDashing = true;
        SetCurrentState(PlayerState.Dashing);

        Vector3 dashDirection = GetMovementDirection();

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward.normalized;
        }

        float dashStartTime = Time.time;
        float dashEndTime = dashStartTime + dashDuration; // Définir la fin du dash en utilisant dashDuration

        MovementDirection dashDirectionRelative = DetermineRelativeMovementDirection(dashDirection);

        switch (dashDirectionRelative)
        {
            case MovementDirection.Forward:
                playerAnimator.CrossFade("DashForward", 0f);
                break;
            case MovementDirection.Backward:
                isDashing = false;
                yield break;
            case MovementDirection.Right:
                playerAnimator.CrossFade("DashRight", 0f);
                break;
            case MovementDirection.Left:
                playerAnimator.CrossFade("DashLeft", 0f);
                break;
        }

        

        dashMeshTrail.DisplayMeshTrail(dashDuration);

        while (Time.time < dashEndTime)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        actionEndTime = Time.time + dashDuration; // Bloquer les autres actions pendant la durée du dash
    }

    private IEnumerator HandleInteraction()
    {
        if (isInteracting || isAttacking || isDashing)
        {
            yield break; // Si déjà en train d'interagir ou d'une autre action, ne fait rien
        }
        playerAnimator.CrossFade("Interact",0.1f);

        isInteracting = true;
        SetCurrentState(PlayerState.Interacting);
        actionEndTime = Time.time + interactionDuration;

        // Attendez la durée de l'interaction
        yield return new WaitForSeconds(interactionDuration);

        isInteracting = false;
    }

    private IEnumerator HandleAttack(int attackType)
    {
        float attackDuration = playerAttacks.CastAttack(attackType);
        if (attackDuration > -0.1f)
        {
            playerAnimator.CrossFade($"Attack{attackType}", 0.1f);
            isAttacking = true;
            SetCurrentState(PlayerState.Attacking);
            yield return new WaitForSeconds(attackDuration);
            isAttacking = false;
        }
    }

    private void HandleMovement()
    {
        if (inputDirection == Vector2.zero)
        {
            SetCurrentState(PlayerState.Idle);
            playerAnimator.Play("Idle");
            relativeMovementDirection = MovementDirection.None;
            return;
        }

        Vector3 moveDirection = GetMovementDirection();
        relativeMovementDirection = DetermineRelativeMovementDirection(moveDirection);

        float speed = isRunning && (relativeMovementDirection != MovementDirection.Backward) ? runSpeed : walkSpeed;

        // Mouvement horizontal uniquement (sans gravité)
        Vector3 horizontalMove = moveDirection * speed;

        characterController.SimpleMove(horizontalMove);

        currentDirection = DetermineMovementDirection(moveDirection);
        SetCurrentState(isRunning && (relativeMovementDirection != MovementDirection.Backward) ? PlayerState.Running : PlayerState.Walking);

        if (currentState == PlayerState.Running)
        {
            switch (relativeMovementDirection)
            {
                case MovementDirection.Forward:
                    playerAnimator.Play("Running");
                    break;
                case MovementDirection.Right:
                    playerAnimator.Play("RunningRight");
                    break;
                case MovementDirection.Left:
                    playerAnimator.Play("RunningLeft");
                    break;
            }
        }
        else if (currentState == PlayerState.Walking)
        {
            switch (relativeMovementDirection)
            {
                case MovementDirection.Forward:
                case MovementDirection.Right:
                case MovementDirection.Left:
                    playerAnimator.Play("WalkingForward");
                    break;
                case MovementDirection.Backward:
                    playerAnimator.Play("WalkingBackwards");
                    break;
            }
        }
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
        //Calculer la position relative par rapport à l'avant du personnage, la caméra ne tourne pas avec le personnage, calcule par rapport à l'input et à la caméra

        if (moveDirection == Vector3.zero)
        {
            return MovementDirection.None;
        }

        float angle = Vector3.SignedAngle(GetCameraForward(), moveDirection, Vector3.up);

        if (angle >= -45 && angle <= 45)
        {
            return MovementDirection.Forward;
        }
        else if (angle >= 45 && angle <= 135)
        {
            return MovementDirection.Right;
        }
        else if (angle <= -45 && angle >= -135)
        {
            return MovementDirection.Left;
        }
        else
        {
            return MovementDirection.Backward;
        }
    }

    private MovementDirection DetermineRelativeMovementDirection(Vector3 moveDirection)
    {
        //Calculer la position relative par rapport à l'avant du personnage, la caméra ne tourne pas avec le personnage, calcule par rapport à l'input et l'avant du transform

        if (moveDirection == Vector3.zero)
        {
            return MovementDirection.None;
        }

        float angle = Vector3.SignedAngle(transform.forward, moveDirection, Vector3.up);

        if (angle >= -45 && angle <= 45)
        {
            return MovementDirection.Forward;
        }
        else if (angle >= 45 && angle <= 135)
        {
            return MovementDirection.Right;
        }
        else if (angle <= -45 && angle >= -135)
        {
            return MovementDirection.Left;
        }
        else
        {
            return MovementDirection.Backward;
        }
    }

    private void RotateTowardsMouse()
    {
        Vector3 directionToMouse = (MouseIndicator.GetMouseWorldPosition() - transform.position).normalized;
        directionToMouse.y = 0; // Ignorer la composante Y pour la rotation horizontale
        Quaternion targetRotation = Quaternion.LookRotation(directionToMouse);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void SetCurrentState(PlayerState newState)
    {
        oldState = currentState;
        currentState = newState;
    }

    private void StartCharging(int attackType)
    {
        if (isAttacking || isDashing || isInteracting)
        {
            return;
        }

        playerAttacks.ShowAttackPreview(attackType);
    }

    private void Attack(int attackType)
    {
        playerAttacks.HideAllAttackPatterns();
        StartCoroutine(HandleAttack(attackType));
    }
}
