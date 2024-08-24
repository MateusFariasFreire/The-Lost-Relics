using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private float followZone = 10f;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float lerpSpeed = 5f;

    private Transform target = null;
    private Vector3 initialDirection;

    void Start()
    {
        // On capture la direction initiale au moment de la création de l'objet
        initialDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;

            // On fait le Lerp entre la direction initiale et la direction vers la cible
            Vector3 newDirection = Vector3.Lerp(initialDirection, targetDirection, lerpSpeed * Time.deltaTime);

            transform.rotation = Quaternion.LookRotation(newDirection);

            if (Vector3.Distance(transform.position, target.position) < 1f)
            {
                Destroy(target.gameObject);
                Destroy(gameObject);
            }

            // On met à jour la direction initiale pour la prochaine itération
            initialDirection = newDirection;
        }
        else
        {
            target = GetClosestEnemy();
        }

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

                Ray ray = new Ray(transform.position, directionToEnemy.normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distanceToEnemy))
                {
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
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Dessine le gizmo de la zone de suivi
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followZone);
    }
}
