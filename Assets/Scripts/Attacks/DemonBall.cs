using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DemonBall : MonoBehaviour
{
    [SerializeField] private float initialImpulse = 5f;
    [SerializeField] private float climbDuration = 1f;
    [SerializeField] private float descentAcceleration = 2f;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float impactRadius = 5f;

    private float postExplosionTimer = 0f;

    private int _damage = 0;

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
            postExplosionTimer += Time.deltaTime;

            if (postExplosionTimer >= 0.5)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius, LayerMask.GetMask("Enemies"));
                foreach (Collider collider in colliders)
                {
                    collider.SendMessage("TakeDamage", _damage / 10);
                }

                postExplosionTimer = 0f;
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

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit something");

        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius, LayerMask.GetMask("Enemies"));
        foreach (Collider collider in colliders)
        {
            collider.SendMessage("TakeDamage", _damage);
        }

        burnEnemiesActive = true;

        GameObject hitEffectGO = Instantiate(hitEffect, transform.position, Quaternion.identity);

        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        rb.isKinematic = true;

        Destroy(hitEffectGO, 1.40f);
        Destroy(gameObject, 1.40f);


        Debug.Log("TriggerExit");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
