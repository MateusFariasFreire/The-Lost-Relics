using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpells", menuName = "The Lost Relics/Player Spells", order = 2)]
public class PlayerSpells : ScriptableObject
{
    [Header("Unlocked Spells")]
    [SerializeField] private bool[] unlockedSpells = new bool[6];

    public bool IsSpellUnlocked(int spellIndex)
    {
        if (spellIndex < 0 || spellIndex >= unlockedSpells.Length)
        {
            Debug.LogWarning("Spell index is out of range!");
            return false;
        }
        return unlockedSpells[spellIndex];
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

