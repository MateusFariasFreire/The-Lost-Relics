using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTornado : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float bumpForce = 5f;

    [SerializeField] GameObject windEffect;


    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Rigidbody>().AddForce(transform.up * bumpForce, ForceMode.Impulse);
    }
}
