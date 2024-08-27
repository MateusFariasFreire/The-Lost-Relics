using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpells", menuName = "The Lost Relics/Player Spells", order = 2)]
public class PlayerSpells : ScriptableObject
{
    [Header("Unlocked Spells")]
    [SerializeField] private bool[] unlockedSpells = new bool[5];

    [SerializeField] private bool[] enabledSpells = new bool[5];

    [Header("Dash")]
    [SerializeField] private bool dashEnabled = false;
    public bool DashEnabled { get { return dashEnabled; } set { dashEnabled = value; } }

    [SerializeField] private ScenarioManager scenarioManager;

    /*public bool IsSpellUnlocked(int spellIndex)
    {
        if (spellIndex < 0 || spellIndex >= enabledSpells.Length)
        {
            Debug.LogWarning("Spell index is out of range!");
            return false;
        }
        return enabledSpells[spellIndex];
    }

    public bool IsSpellEnabled(int spellIndex)
    {
        if (spellIndex < 0 || spellIndex >= enabledSpells.Length)
        {
            Debug.LogWarning("Spell index is out of range!");
            return false;
        }
        return enabledSpells[spellIndex];
    }*/

    public bool CanUseSpell(int spellIndex)
    {
        if (spellIndex < 0 || spellIndex >= enabledSpells.Length)
        {
            Debug.LogWarning("Spell index is out of range!");
            return false;
        }

        if(!(unlockedSpells[spellIndex] && enabledSpells[spellIndex]))
        {
            return false;
        }

        if (!scenarioManager.hasFoundBook)
        {
            return false;
        }

        // 1 = wand , 2 = staff
        int requiredWeapon = 0;

        switch (spellIndex)
        {
            case 0:
            case 1:
            case 4:
                requiredWeapon = 1;
                break;
            case 2:
            case 3:
                requiredWeapon = 2;
                break;
        }

        if (requiredWeapon == 1 && scenarioManager.hasFoundWand)
        {
            return true;
        }

        if(requiredWeapon == 2 && scenarioManager.hasFoundStaff)
        {
            return true;
        }

        return false;
    }

    public void EnableSpell(int spellIndex)
    {
        if (spellIndex < 0 || spellIndex >= enabledSpells.Length)
        {
            Debug.LogWarning("Spell index is out of range!");
            return;
        }
        enabledSpells[spellIndex] = true;
    }

    public void UnlockSpell(int spellIndex)
    {
        if (spellIndex < 0 || spellIndex >= unlockedSpells.Length)
        {
            Debug.LogWarning("Spell index is out of range!");
            return;
        }
        unlockedSpells[spellIndex] = true;
    }

}

