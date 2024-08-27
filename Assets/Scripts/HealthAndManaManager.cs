using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerController;

public class HealthAndManaManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [Header("UI")]
    [SerializeField] private UIBarController healthBarController;
    [SerializeField] private UIBarController manaBarController;

    [SerializeField] private int healthRegenPerSecond = 1;
    [SerializeField] private int manaRegenPerSecond = 1;

    private float healthRegenTimer = 0f;
    private float manaRegenTimer = 0f;

    private void Start()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }
    
    private void Update()
    {
        healthRegenTimer += Time.deltaTime;
        manaRegenTimer += Time.deltaTime;
        
        if (healthRegenTimer > 1f) {
            if (playerStats.HP < playerStats.HPMax)
            {
                playerStats.HP = Mathf.Clamp(playerStats.HP + healthRegenPerSecond, 0, playerStats.HPMax);
                healthRegenTimer = 0f;
                UpdateHealthBar();
            }
        }

        if (manaRegenTimer > 1f)
        {
            if (playerStats.Mana < playerStats.ManaMax)
            {
                playerStats.Mana = Mathf.Clamp(playerStats.Mana + manaRegenPerSecond, 0, playerStats.ManaMax);
                manaRegenTimer = 0f;
                UpdateManaBar();
            }
        }
        

    }

    public bool TakeDamage(int damage)
    {
        bool isDead = false;
        playerStats.HP -= damage;
        if (playerStats.HP <= 0)
        {

            playerStats.HP = playerStats.HPMax;
            playerStats.Mana = playerStats.ManaMax;

            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
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

    public bool CanUseSpell(int manaCost)
    {
        return playerStats.Mana >= manaCost;
    }
}
