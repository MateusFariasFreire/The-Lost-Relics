using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravity : MonoBehaviour
{
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 1.1f;

    private bool isGrounded = false;

    void Update()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundLayer);

        if (!isGrounded)
        {
            transform.position += Vector3.down * gravity * Time.deltaTime;
        }
        else
        {
            float groundHeight = hit.point.y;
            float playerHeight = transform.localScale.y / 2;
            transform.position = new Vector3(transform.position.x, groundHeight + playerHeight, transform.position.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
    }
}
