using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class HealthAndManaManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [Header("UI")]
    [SerializeField] private UIBarController healthBarController;
    [SerializeField] private UIBarController manaBarController;

    private void Start()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }

    public bool TakeDamage(int damage)
    {
        bool isDead = false;
        playerStats.HP -= damage;
        if (playerStats.HP <= 0)
        {
            playerStats.HP = 0;
            Debug.Log("Player is dead");
            isDead = true;
        }

        UpdateHealthBar();

        return isDead;
    }

    private void UpdateHealthBar()
    {
        healthBarController.SetPercent(playerStats.HP, playerStats.HPMax);
    }

    private void UpdateManaBar()
    {
        manaBarController.SetPercent(playerStats.Mana, playerStats.ManaMax);
    }

    public void Heal(int amount)
    {
        playerStats.HP += amount;
        if (playerStats.HP > playerStats.HPMax)
        {
            playerStats.HP = playerStats.HPMax;
        }

        UpdateHealthBar();
    }

    public void RestoreMana(int amount)
    {
        playerStats.Mana += amount;
        if (playerStats.Mana > playerStats.ManaMax)
        {
            playerStats.Mana = playerStats.ManaMax;
        }
    }

    public void UseMana(int amount)
    {
        if (playerStats.Mana >= amount)
        {
            playerStats.Mana -= amount;
        }

        UpdateManaBar();
    }

    public void IncreaseMaxHP(int amount)
    {
        playerStats.HPMax += amount;
        UpdateHealthBar();
    }

    public void IncreaseMaxMana(int amount)
    {
        playerStats.ManaMax += amount;
        UpdateManaBar();
    }

    public void IncreaseDamage(int amount)
    {
        playerStats.DamagePercent += amount;
    }

    public void ResetStats()
    {
        playerStats.HP = playerStats.HPMax;
        playerStats.Mana = playerStats.ManaMax;

        UpdateHealthBar();
        UpdateManaBar();
    }
}
