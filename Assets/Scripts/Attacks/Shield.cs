using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    private GameObject player;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float force = 5f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 direction = other.transform.position - transform.position;
        other.GetComponent<Rigidbody>().AddForce(direction.normalized * force * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
