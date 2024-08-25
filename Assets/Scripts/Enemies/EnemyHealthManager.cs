using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] private int health = 50;
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private float timeBeforeDestroy = 5f;
    [SerializeField] private Canvas healtBarCanvas;
    [SerializeField] private Image healthBar;
    bool isDead = false;

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.fillAmount = (float)health / maxHealth;
        if (!isDead && health <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        if (!isDead && health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        GetComponent<EnemyController>().SetDead(true);
        healtBarCanvas.enabled = false;

        Animator animator = GetComponent<Animator>(); 
        CharacterController characterController = GetComponent<CharacterController>();
        characterController.detectCollisions = false;

        animator.CrossFade("Death",0.2f);

        StartCoroutine(DestroyEnemy());
    }

    private IEnumerator DestroyEnemy()
    {
        CharacterController characterController = GetComponent<CharacterController>();

        float timeUntilGrounded = 0f;
        while (characterController.isGrounded == false)
        {
            timeUntilGrounded += Time.deltaTime;
            yield return null;
        }

        GetComponent<EnemyController>().enabled = false;
        GetComponent<CharacterController>().enabled = false;

        yield return new WaitForSeconds(timeBeforeDestroy - timeUntilGrounded);
        Destroy(gameObject);
    }


}
