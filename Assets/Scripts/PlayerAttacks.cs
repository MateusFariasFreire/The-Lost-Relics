using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Transform staffSpawnPoint;
    private Animator animator;
    private PlayerController playerController;

    [Header("Attacks")]
    [SerializeField] private List<GameObject> attacks;
    [SerializeField] private List<Attack> attacksManagers;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        foreach (GameObject attack in attacks)
        {
            GameObject instatiatedAttack = Instantiate(attack, transform);
            attacksManagers.Add(instatiatedAttack.GetComponent<Attack>());
        }
    }

    public float CastAttack(int attackType)
    {
        if (attackType > attacksManagers.Count)
        {
            return -1f;

        }
        return attacksManagers[attackType-1].Cast(staffSpawnPoint.position);
    }

    public void ShowAttackPreview(int attackType)
    {
        if (attackType > attacksManagers.Count)
        {
            return;

        }
        attacksManagers[attackType-1].ShowPreview();
    }
    
    public void HideAllAttackPatterns()
    {
        foreach (Attack attack in attacksManagers)
        {
            attack.HideCastZone();
        }
    }

    private bool IsOnCooldown(int attackType)
    {
        if (attackType > attacksManagers.Count)
        {
            return false;

        }
        return attacksManagers[attackType-1].CanCast;
    }
}
