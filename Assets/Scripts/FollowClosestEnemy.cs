using System.Collections.Generic;
using UnityEngine;

public class FollowClosestEnemy : MonoBehaviour
{
    [SerializeField] private float followZone = 10f;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float lerpSpeed = 5f;  // Vitesse du Lerp pour rendre le mouvement plus smooth

    private Transform target = null;

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
            return;
        }


        if (target != null)
        {
            // Interpoler la direction de la balle vers l'ennemi pour un suivi plus fluide
            Vector3 targetDirection = (target.position - transform.position).normalized;
            Vector3 newDirection = Vector3.Lerp(transform.forward, targetDirection, lerpSpeed * Time.deltaTime);

            // Applique la nouvelle direction
            transform.rotation = Quaternion.LookRotation(newDirection);

            if (Vector3.Distance(transform.position, target.position) < 1f)
            {
                Debug.Log("Hit enemy!");
                Destroy(gameObject);
            }
        }
        else
        {
            target = GetClosestEnemy();
        }

        // Avance la balle dans la direction actuelle
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    Transform GetClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, followZone);
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = hitCollider.transform.position - transform.position;
                float distanceToEnemy = directionToEnemy.magnitude;

                // Vérifie s'il n'y a absolument rien entre la balle et l'ennemi
                Ray ray = new Ray(transform.position, directionToEnemy.normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distanceToEnemy))
                {
                    // S'assure que l'objet touché par le Raycast est bien l'ennemi
                    if (hit.collider.transform == hitCollider.transform)
                    {
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            closestEnemy = hitCollider.transform;
                        }
                    }
                }
            }
        }

        return closestEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit");
            Destroy(gameObject);  // Détruit la balle lorsqu'elle touche un ennemi
        }
        else
        {
            Debug.Log("Not an enemy");
            Destroy(gameObject);  // Détruit la balle lorsqu'elle touche autre chose qu'un ennemi
        }
    }

    // Dessine le gizmo de la zone de suivi
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followZone);
    }
}
