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
        Attacking1,
        Attacking2,
        Attacking3,
        Attacking4,
        Attacking5
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
    [SerializeField] private float attackDuration1 = 1f;
    [SerializeField] private float attackDuration2 = 1f;
    [SerializeField] private float attackDuration3 = 1f;
    [SerializeField] private float attackDuration4 = 1f;
    [SerializeField] private float attackDuration5 = 1f;
    [SerializeField] private float interactionDuration = 2f;

    [Header("Attack Cooldowns")]
    [SerializeField] private float attackCooldown1 = 2f;
    [SerializeField] private float attackCooldown2 = 3f;
    [SerializeField] private float attackCooldown3 = 4f;
    [SerializeField] private float attackCooldown4 = 5f;
    [SerializeField] private float attackCooldown5 = 5f;

    [Header("Dash Effect")]
    [SerializeField] private MeshTrail dashMeshTrail;

    private PlayerState currentState = PlayerState.Idle;
    private PlayerState oldState = PlayerState.Idle;
    private MovementDirection currentDirection = MovementDirection.None;
    private MovementDirection relativeMovementDirection = MovementDirection.None;

    private Vector2 inputDirection;
    private bool isRunning;
    private bool isDashing;
    private bool isInteracting;
    private bool isAttacking;

    private float actionEndTime = 0f;
    private float attackEndTime = 0f;
    private float attackCooldownEndTime1 = 0f;
    private float attackCooldownEndTime2 = 0f;
    private float attackCooldownEndTime3 = 0f;
    private float attackCooldownEndTime4 = 0f;
    private float attackCooldownEndTime5 = 0f;

    Animator playerAnimator;
    private PlayerAttacks playerAttacksManager;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        playerAttacksManager = GetComponent<PlayerAttacks>();
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
    public void OnAttack5(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed && Time.time > attackCooldownEndTime4)
        {
            StartCoroutine(PerformAttack(5));
        }
    }

    private IEnumerator PerformAttack(int attackType)
    {
        if (isAttacking)
        {
            yield break; // Si déjà en train d'attaquer, ne fait rien
        }

        isAttacking = true;

        // Définir la durée de l'attaque et la fin du cooldown en fonction du type d'attaque
        float attackDuration = 0f;
        float attackCooldown = 0f;

        Vector3 mouseWolrdPosOnCast = MouseIndicator.GetMouseWorldPosition();

        switch (attackType)
        {
            case 1:
                SetCurrentState(PlayerState.Attacking1);
                playerAnimator.CrossFade("Attack1", 0.2f);
                playerAttacksManager.CastAttack1();
                attackDuration = attackDuration1;
                attackCooldown = attackCooldown1;

                break;
            case 2:
                SetCurrentState(PlayerState.Attacking2);
                playerAnimator.CrossFade("Attack2", 0.2f);
                attackDuration = attackDuration2;
                attackCooldown = attackCooldown2;

                yield return new WaitForSeconds(attackDuration / 2);
                if (mouseWolrdPosOnCast != Vector3.zero)
                {
                    playerAttacksManager.CastAttack2(mouseWolrdPosOnCast);
                }

                yield return new WaitForSeconds(attackDuration / 2);


                break;
            case 3:
                SetCurrentState(PlayerState.Attacking3);
                playerAnimator.CrossFade("Attack3", 0.2f);
                attackDuration = attackDuration3;
                attackCooldown = attackCooldown3;

                yield return new WaitForSeconds(attackDuration / 2);
                if (mouseWolrdPosOnCast != Vector3.zero)
                {
                    playerAttacksManager.CastAttack3();
                }

                yield return new WaitForSeconds(attackDuration / 2);
                break;
            case 4:
                SetCurrentState(PlayerState.Attacking4);
                playerAnimator.CrossFade("Attack4", 0.2f);
                attackDuration = attackDuration4;
                attackCooldown = attackCooldown4;


                yield return new WaitForSeconds(attackDuration / 2);
                if (mouseWolrdPosOnCast != Vector3.zero)
                {
                    playerAttacksManager.CastAttack4();
                }

                yield return new WaitForSeconds(attackDuration / 2);
                break;
            case 5:
                SetCurrentState(PlayerState.Attacking4);
                playerAnimator.CrossFade("Attack5", 0.2f);
                attackDuration = attackDuration5;
                attackCooldown = attackCooldown5;

                yield return new WaitForSeconds(attackDuration / 2);
                if (mouseWolrdPosOnCast != Vector3.zero)
                {
                    playerAttacksManager.CastAttack5();
                }

                yield return new WaitForSeconds(attackDuration / 2);
                break;
        }

        attackEndTime = Time.time + attackDuration;
        actionEndTime = attackEndTime;
        isAttacking = false;

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

            case 5:
                attackCooldownEndTime5 = Time.time + attackCooldown;
                break;
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
        playerAnimator.Play("Interact");

        isInteracting = true;
        SetCurrentState(PlayerState.Interacting);
        actionEndTime = Time.time + interactionDuration;

        // Attendez la durée de l'interaction
        yield return new WaitForSeconds(interactionDuration);

        isInteracting = false;
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

        characterController.Move(moveDirection * speed * Time.deltaTime);
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
}
