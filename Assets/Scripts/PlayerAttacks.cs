using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private PlayerSpells playerSpells;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        foreach (GameObject attack in attacks)
        {
            GameObject instatiatedAttack = Instantiate(attack, transform);
            attacksManagers.Add(instatiatedAttack.GetComponent<Attack>());
        }
    }

    public float CastAttack(int attackNumber)
    {
        if (attackNumber > attacksManagers.Count || !playerSpells.CanUseSpell(attackNumber))
        {
            return -1f;

        }
        return attacksManagers[attackNumber].Cast(staffSpawnPoint.position);
    }

    public void ShowAttackPreview(int attackNumber)
    {
        if (attackNumber > attacksManagers.Count || !playerSpells.CanUseSpell(attackNumber))
        {
            return;

        }
        attacksManagers[attackNumber].ShowPreview();
    }
    
    public void HideAllAttackPatterns()
    {
        foreach (Attack attack in attacksManagers)
        {
            attack.HideCastZone();
        }
    }

    private bool IsOnCooldown(int attackNumber)
    {
        if (attackNumber > attacksManagers.Count)
        {
            return false;

        }
        return attacksManagers[attackNumber].CanCast;
    }
}
