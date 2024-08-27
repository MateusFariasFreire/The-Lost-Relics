using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Update()
    {
        transform.position = new Vector3(target.position.x, transform.position.y , target.position.z);
    }
}
