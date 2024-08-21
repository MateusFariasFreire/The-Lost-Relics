using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 10f;
    public float dashDistance = 5f;
    public float dashSpeed = 20f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.5f;
    public float groundStickiness = 5f;
    public bool canInteract = false;

    private CharacterController characterController;
    private Camera mainCamera;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool isRunning;
    private Vector3 velocity;
    private bool dashRequested;
    private bool isDashing;
    private bool isDefending;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Move();
        RotateTowardsMouse();
        ApplyGravity();

        // Gestion du dash
        if (dashRequested && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    public Vector2 GetMovementInput()
    {
        return movementInput;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public bool IsDefending()
    {
        return isDefending;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        movementInput = value.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        lookInput = value.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext value)
    {
        isRunning = value.phase == InputActionPhase.Performed;
    }

    public void OnDash(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            dashRequested = true;
        }
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed && canInteract)
        {
            Debug.Log("Interaction effectuée !");
            GetComponent<PlayerAnimationController>().PlayInteractAnimation();
        }
    }

    public void OnAttack1(InputAction.CallbackContext value) => TriggerAttack(value, 1);
    public void OnAttack2(InputAction.CallbackContext value) => TriggerAttack(value, 2);
    public void OnAttack3(InputAction.CallbackContext value) => TriggerAttack(value, 3);
    public void OnAttack4(InputAction.CallbackContext value) => TriggerAttack(value, 4);

    private void TriggerAttack(InputAction.CallbackContext value, int attackIndex)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            Debug.Log($"Attaque {attackIndex} exécutée !");
            GetComponent<PlayerAnimationController>().PlayAttackAnimation(attackIndex);
        }
    }

    public void OnDefend(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            isDefending = true;
            Debug.Log("Mode défense activée");
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            isDefending = false;
            Debug.Log("Mode défense désactivé");
        }
    }

    private void Move()
    {
        if (isDashing) return;

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * movementInput.y + right * movementInput.x;
        direction.Normalize();

        float speed = isRunning ? runSpeed : walkSpeed;
        characterController.Move(direction * speed * Time.deltaTime);
    }

    private void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void ApplyGravity()
    {
        if (IsGrounded(out RaycastHit hit))
        {
            float distanceToGround = transform.position.y - hit.point.y;
            if (distanceToGround > 0.01f)
            {
                velocity.y = Mathf.Lerp(velocity.y, gravity, Time.deltaTime * groundStickiness);
            }
            else
            {
                velocity.y = 0f;
            }

            Vector3 targetPosition = new Vector3(transform.position.x, hit.point.y + characterController.skinWidth, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * groundStickiness);
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private bool IsGrounded(out RaycastHit hit)
    {
        return Physics.Raycast(transform.position + Vector3.up * characterController.skinWidth, Vector3.down, out hit, groundCheckDistance + characterController.skinWidth);
    }

    private System.Collections.IEnumerator PerformDash()
    {
        // Initialisation du dash
        isDashing = true;
        dashRequested = false;

        Vector3 dashDirection = new Vector3(movementInput.x, 0, movementInput.y);
        dashDirection = mainCamera.transform.TransformDirection(dashDirection);
        dashDirection.y = 0f;
        dashDirection.Normalize();

        float dashTime = dashDistance / dashSpeed;
        float elapsedTime = 0;

        while (elapsedTime < dashTime)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fin du dash
        isDashing = false;
    }
}
