using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBall : MonoBehaviour
{
    [SerializeField] private float attractionForce = 10f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float groundDistance = 3f;

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }

        FollowMouse();

    }

    private void FollowMouse()
    {
        Vector3 mousePos = MouseIndicator.GetMouseWorldPosition();
        mousePos.y = mousePos.y + groundDistance;
        transform.position = Vector3.Lerp(transform.position, mousePos, lerpSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("here");

        Vector3 direction = transform.position - other.transform.position;
        other.GetComponent<Rigidbody>().AddForce(direction.normalized * attractionForce * Time.deltaTime, ForceMode.Impulse);
    }
}
