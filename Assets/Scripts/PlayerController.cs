using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 10f;

    private CharacterController characterController;
    private Camera mainCamera;

    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool isRunning;

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
    }

    // Appelée par l'événement "Move"
    public void OnMove(InputAction.CallbackContext value)
    {
        movementInput = value.ReadValue<Vector2>();
    }

    // Appelée par l'événement "Look"
    public void OnLook(InputAction.CallbackContext value)
    {
        lookInput = value.ReadValue<Vector2>();
    }

    // Appelée par l'événement "Run"
    public void OnRun(InputAction.CallbackContext value)
    {
        isRunning = value.phase == InputActionPhase.Performed;
    }

    private void Move()
    {
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
            lookDirection.y = 0f; // Garder la rotation dans le plan horizontal

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
