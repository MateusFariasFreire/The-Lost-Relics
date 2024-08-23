using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Transform staffSpawnPoint;
    private Animator animator;
    private PlayerController playerController;
    [SerializeField] private Material indicatorActiveMaterial;
    [SerializeField] private Material indicatorInactiveMaterial;

    [Header("Attack 1")]
    [SerializeField] private GameObject attack1Prefab;
    [SerializeField] private float attack1AnimDuration = 1f;
    [SerializeField] private float attackCooldown1 = 2f;

    [Header("Attack 2")]
    [SerializeField] private GameObject attack2Prefab;
    [SerializeField] private float attack2AnimDuration = 1f;
    [SerializeField] private float attackCooldown2 = 3f;
    [SerializeField] private float attack2Height = 1f;
    [SerializeField] private Canvas attack2Canvas;

    [Header("Attack 3")]
    [SerializeField] private GameObject attack3Prefab;
    [SerializeField] private float attack3AnimDuration = 1f;
    [SerializeField] private float attackCooldown3 = 4f;
    [SerializeField] private Canvas attack3Canvas;

    [Header("Attack 4")]
    [SerializeField] private GameObject attack4Prefab;
    [SerializeField] private float attack4AnimDuration = 1f;
    [SerializeField] private float attackCooldown4 = 5f;
    [SerializeField] private Canvas attack4Canvas;

    [Header("Attack 5")]
    [SerializeField] private GameObject attack5Prefab;
    [SerializeField] private float attack5AnimDuration = 1f;
    [SerializeField] private float attackCooldown5 = 5f;

    private Dictionary<int, float> attackCooldownEndTimes = new Dictionary<int, float>();


    private void Start()
    {
        attack2Canvas.enabled = false;
        attack3Canvas.enabled = false;
        attack4Canvas.enabled = false;

        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (attack2Canvas.enabled)
        {
            UpdateMaterial(attack2Canvas.transform.Find("Image").GetComponent<Image>(), 2);
            attack2Canvas.transform.position = MouseIndicator.GetMouseWorldPosition();
        }

        if (attack3Canvas.enabled)
        {
            UpdateMaterial(attack3Canvas.transform.Find("Image").GetComponent<Image>(), 3);
            Quaternion attack3Rotation = Quaternion.LookRotation(MouseIndicator.GetMouseWorldPosition() - transform.position);
            attack3Rotation.eulerAngles = new Vector3(0, attack3Rotation.eulerAngles.y, attack3Rotation.eulerAngles.z);
            attack3Canvas.transform.rotation = Quaternion.Lerp(attack3Rotation, attack3Canvas.transform.rotation, 0);
        }

        if (attack4Canvas.enabled)
        {
            UpdateMaterial(attack4Canvas.transform.Find("Image").GetComponent<Image>(), 4);
            attack4Canvas.transform.position = MouseIndicator.GetMouseWorldPosition();
        }
    }

    public float CastAttack(int attackType)
    {
        if (IsOnCooldown(attackType) && playerController.CurrentState != PlayerController.PlayerState.Idle) return 0f;

        switch (attackType)
        {
            case 1:
                StartCoroutine(PerformAttack1());
                return attack1AnimDuration;
            case 2:
                StartCoroutine(PerformAttack2(MouseIndicator.GetMouseWorldPosition()));
                return attack2AnimDuration;
            case 3:
                StartCoroutine(PerformAttack3());
                return attack3AnimDuration - 0.2f;
            case 4:
                StartCoroutine(PerformAttack4());
                return attack4AnimDuration - 0.2f;
            case 5:
                StartCoroutine(PerformAttack5());
                return attack5AnimDuration - 0.2f;
            default:
                return 0f;
        }
    }

    private IEnumerator PerformAttack1()
    {
        animator.CrossFade("Attack1", 0.1f);

        yield return new WaitForSeconds(attack1AnimDuration / 2);

        GameObject attack1 = Instantiate(attack1Prefab, staffSpawnPoint.position, transform.rotation);
        SetCooldown(1, attackCooldown1);

        yield return new WaitForSeconds(attack1AnimDuration / 2);
    }

    private IEnumerator PerformAttack2(Vector3 targetPosition)
    {
        animator.CrossFade("Attack2", 0.1f);

        yield return new WaitForSeconds(attack2AnimDuration / 2);

        targetPosition.Set(targetPosition.x, targetPosition.y + attack2Height, targetPosition.z);
        GameObject attack2 = Instantiate(attack2Prefab, targetPosition, Quaternion.identity);
        SetCooldown(2, attackCooldown2);

        yield return new WaitForSeconds(attack2AnimDuration / 2);
    }

    private IEnumerator PerformAttack3()
    {
        animator.CrossFade("Attack3", 0.1f);

        yield return new WaitForSeconds(attack3AnimDuration / 2 - 0.1f);

        GameObject attack3 = Instantiate(attack3Prefab, staffSpawnPoint.position, transform.rotation);
        SetCooldown(3, attackCooldown3);

        yield return new WaitForSeconds(attack3AnimDuration / 2 + 0.1f);
    }

    private IEnumerator PerformAttack4()
    {
        animator.CrossFade("Attack4", 0.1f);

        yield return new WaitForSeconds(attack4AnimDuration / 2);

        GameObject attack4 = Instantiate(attack4Prefab, staffSpawnPoint.position, transform.rotation);
        SetCooldown(4, attackCooldown4);

        yield return new WaitForSeconds(attack4AnimDuration / 2);
    }

    private IEnumerator PerformAttack5()
    {
        animator.CrossFade("Attack5", 0.1f);

        yield return new WaitForSeconds(attack5AnimDuration / 2);

        GameObject shield = Instantiate(attack5Prefab, transform.position, transform.rotation);
        SetCooldown(5, attackCooldown5);

        yield return new WaitForSeconds(attack5AnimDuration / 2);
    }

    public void DisplayAttack2Pattern()
    {
        attack2Canvas.enabled = true;
    }

    public void DisplayAttack3Pattern()
    {
        attack3Canvas.enabled = true;
    }

    public void DisplayAttack4Pattern()
    {
        attack4Canvas.enabled = true;
    }

    public void HideAllAttackPatterns()
    {
        attack2Canvas.enabled = false;
        attack3Canvas.enabled = false;
        attack4Canvas.enabled = false;
    }

    private bool IsOnCooldown(int attackType)
    {
        if (attackCooldownEndTimes.ContainsKey(attackType) && Time.time < attackCooldownEndTimes[attackType])
        {
            return true;
        }
        return false;
    }

    private void SetCooldown(int attackType, float cooldownDuration)
    {
        attackCooldownEndTimes[attackType] = Time.time + cooldownDuration;
    }

    private void UpdateMaterial(Image image, int attackNumber)
    {
        if (IsOnCooldown(attackNumber))
        {
            image.material = indicatorInactiveMaterial;
        }
        else
        {
            image.material = indicatorActiveMaterial;
        }
    }
}
