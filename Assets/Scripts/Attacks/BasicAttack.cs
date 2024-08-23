using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private float followZone = 10f;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float lerpSpeed = 5f;

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
            Vector3 targetDirection = (target.position - transform.position).normalized;
            Vector3 newDirection = Vector3.Lerp(transform.forward, targetDirection, lerpSpeed * Time.deltaTime);

            transform.rotation = Quaternion.LookRotation(newDirection);

            if (Vector3.Distance(transform.position, target.position) < 1f)
            {
                Destroy(target.gameObject);
                Destroy(gameObject);
            }
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
