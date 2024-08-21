using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        UpdateMovementAnimations();
        UpdateDefendAnimation();
        UpdateDashAnimation();
    }

    private void UpdateMovementAnimations()
    {
        Vector2 movementInput = playerController.GetMovementInput();
        bool isRunning = playerController.IsRunning();

        if (movementInput == Vector2.zero && !playerController.IsDashing() && !playerController.IsDefending())
        {
            animator.SetTrigger("Idle");
        }
        else if (movementInput.y > 0)
        {
            animator.SetTrigger(isRunning ? "RunForward" : "WalkForward");
        }
        else if (movementInput.y < 0)
        {
            animator.SetTrigger("WalkBackward");
        }
        else if (movementInput.x > 0)
        {
            animator.SetTrigger(isRunning ? "RunRight" : "WalkForward");
        }
        else if (movementInput.x < 0)
        {
            animator.SetTrigger(isRunning ? "RunLeft" : "WalkForward");
        }
        else
        {
            animator.ResetTrigger("WalkForward");
            animator.ResetTrigger("WalkBackward");
            animator.ResetTrigger("WalkRight");
            animator.ResetTrigger("WalkLeft");
            animator.ResetTrigger("RunForward");
            animator.ResetTrigger("RunRight");
            animator.ResetTrigger("RunLeft");
        }
    }

    private void UpdateDefendAnimation()
    {
        if (playerController.IsDefending())
        {
            animator.SetTrigger("DefendStart");
            animator.SetBool("IsDefending", true);
        }
        else
        {
            animator.SetBool("IsDefending", false);
        }
    }

    private void UpdateDashAnimation()
    {
        if (playerController.IsDashing())
        {
            Vector2 movementInput = playerController.GetMovementInput();
            if (movementInput.y > 0)
            {
                animator.SetTrigger("DashForward");
            }
            else if (movementInput.y < 0)
            {
                animator.SetTrigger("DashBackward");
            }
            else if (movementInput.x > 0)
            {
                animator.SetTrigger("DashRight");
            }
            else if (movementInput.x < 0)
            {
                animator.SetTrigger("DashLeft");
            }
        }
    }

    public void PlayAttackAnimation(int attackIndex)
    {
        animator.SetTrigger($"Attack{attackIndex}");
    }

    public void PlayInteractAnimation()
    {
        animator.SetTrigger("Interact");
    }
}
