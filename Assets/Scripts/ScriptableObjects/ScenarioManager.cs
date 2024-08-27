using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsScriptableObject", menuName = "The Lost Relics/Scenario Manager", order = 1)]
public class ScenarioManager : ScriptableObject
{
    public bool hasFoundBook = false;
    public bool hasFoundWand = false;
    public bool hasFoundStaff = false;
    public bool hasDefeatedBoss = false;

    [Header("Player Spells")]
    [SerializeField] private PlayerSpells playerSpells;


    public void OnBookFound()
    {
        hasFoundBook = true;
        EnableDash();
    }

    public void OnWandFound()
    {
        hasFoundWand = true;
    }

    public void OnStaffFound()
    {
        hasFoundStaff = true;
        UnlockSpell(0);
        UnlockSpell(1);
        UnlockSpell(2);
        UnlockSpell(3);
        UnlockSpell(4);
        UnlockSpell(5);

        EnableSpell(0);
        EnableSpell(1);
        EnableSpell(2);
        EnableSpell(3);
        EnableSpell(4);
        EnableSpell(5);
    }

    private void EnableDash()
    {
        playerSpells.DashEnabled = true;
    }

    public void UnlockSpell(int spellId)
    {
        playerSpells.UnlockSpell(spellId);
    }


    public void EnableSpell(int spellId)
    {
        playerSpells.EnableSpell(spellId);
    }
}