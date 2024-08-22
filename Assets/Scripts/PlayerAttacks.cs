using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{

    [SerializeField] private GameObject attack1Prefab;
    [SerializeField] private Transform attack1SpawnPoint;


    [SerializeField] private GameObject attack2Prefab;
    [SerializeField] private float attack2Speed = 10f;
    [SerializeField] private int attack2Angle = 45;
    public void CastAttack1()
    {
        GameObject attack1 = Instantiate(attack1Prefab, attack1SpawnPoint.position, transform.rotation);
    }

    public void CastAttack2(Vector3 targetPosition)
    {
        GameObject fireball = Instantiate(attack2Prefab, attack1SpawnPoint.position, Quaternion.identity);


        Vector3 direction = targetPosition - attack1SpawnPoint.position;
        float distanceXZ = new Vector3(direction.x, 0, direction.z).magnitude;
        float yOffset = direction.y;


        // Calculer l'angle en radians
        float radianAngle = Mathf.Deg2Rad * attack2Angle;

        // Calculer la composante horizontale de la vitesse
        float velocityXZ = attack2Speed * Mathf.Cos(radianAngle);

        // Calculer le temps nécessaire pour atteindre la cible
        float time = distanceXZ / velocityXZ;

        // Calculer la composante verticale de la vitesse nécessaire pour compenser la gravité
        float velocityY = attack2Speed * Mathf.Sin(radianAngle) - 0.5f * Physics.gravity.magnitude * time;

        // Combiner les composantes pour obtenir la vélocité totale
        Vector3 velocity = new Vector3(
            direction.x / time,
            velocityY,
            direction.z / time
        );

        // Appliquer la vélocité calculée au Rigidbody de la boule de feu
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }
}
