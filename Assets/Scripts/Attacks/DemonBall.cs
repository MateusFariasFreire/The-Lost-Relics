using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DemonBall : MonoBehaviour
{
    [SerializeField] private float initialImpulse = 5f;
    [SerializeField] private float climbDuration = 1f;
    [SerializeField] private float descentAcceleration = 2f;
    [SerializeField] private GameObject hitEffect;

    [SerializeField] private float impactRadius = 5f;

    private Rigidbody rb;
    private bool isDescending = false;

    private bool burnEnemiesActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(AnimateProjectile());
    }

    private void Update()
    {
        if (burnEnemiesActive)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    private IEnumerator AnimateProjectile()
    {
        rb.velocity = new Vector3(0, initialImpulse, 0);

        yield return new WaitForSeconds(climbDuration);

        isDescending = true;

        while (isDescending && !burnEnemiesActive)
        {
            rb.velocity += new Vector3(0, -descentAcceleration * Time.deltaTime, 0);
            yield return null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject hitEffectGO = Instantiate(hitEffect, transform.position, Quaternion.identity);

        burnEnemiesActive = true;

        GetComponent<MeshRenderer>().enabled = false;
        rb.isKinematic = true;

        Destroy(hitEffectGO, 1.40f);
        Destroy(gameObject, 1.40f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
