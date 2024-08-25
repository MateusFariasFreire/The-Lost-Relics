using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsScriptableObject", menuName = "The Lost Relics/Player Stats", order = 1)]
public class PlayerStats : ScriptableObject
{
    [Header("Health")]
    [SerializeField] private int hp = 100;
    public int HP { get { return hp; } set { hp = value; } }

    [SerializeField] private int hpMax = 100;
    public int HPMax { get { return hpMax; } set { hpMax = value; } }


    [Header("Mana")]
    [SerializeField] private int mana = 100;
    public int Mana { get { return mana; } set { mana = value; } }

    [SerializeField] private int manaMax = 100;
    public int ManaMax { get { return manaMax; } set { manaMax = value; } }

    [Header("Damage")]
    [SerializeField] private int damagePercent = 100;
    public int DamagePercent { get { return damagePercent; } set { damagePercent = value; } }

    [Header("Dash")]
    [SerializeField] private bool dashUnlocked = false;
    public bool DashUnlocked { get { return dashUnlocked; } set { dashUnlocked = value; } }
}
