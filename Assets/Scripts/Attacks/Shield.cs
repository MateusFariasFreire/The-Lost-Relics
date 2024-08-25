using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float baseForce = 5f; // Force de base appliquée aux ennemis à la distance minimale
    private float damageCooldown = 0f;

    private int _damage = 0;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        damageCooldown -= Time.deltaTime;
        if (damageCooldown <=0f)
        {
            //get all enemies in the radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemies"));
            foreach (Collider collider in colliders)
            {
                collider.SendMessage("TakeDamage", _damage);
            }
            damageCooldown = 1f;
        }

        // Positionner le bouclier au niveau du joueur
        transform.position = player.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterController character = other.GetComponent<CharacterController>();

            if (character != null)
            {

                Vector3 direction = other.transform.position - transform.position;
                direction.y = 0; // Ignorer la composante verticale pour une application horizontale de la force

                float distance = direction.magnitude;
                float normalizedDistance = Mathf.Clamp01(distance / radius); // Normaliser la distance (0 à 1)

                // Calculer la force en fonction de la distance
                float force = baseForce * (1 - normalizedDistance);

                // Appliquer une force d'attraction uniforme
                Vector3 push = direction.normalized * force * Time.deltaTime;

                // Déplacer le CharacterController directement
                character.Move(push);
            }
        }
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
