using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private float followZone = 10f;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float lerpSpeed = 5f;

    private int _damage = 0;

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
            Vector3 newDirection = Vector3.Lerp(initialDirection, targetDirection, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(newDirection);
            initialDirection = newDirection;

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                target.SendMessage("TakeDamage", _damage);
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject);
            }
        }
        else
        {
            target = GetClosestEnemy();
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    Transform GetClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, followZone, LayerMask.GetMask("Enemies"));
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            RaycastHit hit;
            Vector3 directionToEnemy = (collider.transform.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, directionToEnemy, out hit, followZone, LayerMask.GetMask("Environment")))
            {
                if (hit.distance < distance)
                {
                    continue;
                }
            }

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = collider.transform;
            }
        }

        return closestEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.SendMessage("TakeDamage", _damage);
        }

        Destroy(gameObject);
    }

    // Dessine le gizmo de la zone de suivi
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followZone);
    }
}
