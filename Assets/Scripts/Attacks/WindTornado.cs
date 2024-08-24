using System.Collections;
using UnityEngine;

public class WindTornado : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float bumpHeight = 5f; // Hauteur à atteindre
    [SerializeField] private float airDuration = 0.5f; // Durée à maintenir en l'air
    [SerializeField] private float descendSpeed = 2f; // Vitesse de descente
    [SerializeField] private float holdDuration = 0.2f; // Durée supplémentaire pour maintenir en l'air avant la descente

    [SerializeField] GameObject windEffect;

    private void Update()
    {
        // Déplace le tornado vers l'avant
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.GetComponent<CharacterController>();

        if (character != null)
        {
            StartCoroutine(LiftAndDescend(character));
        }
    }

    private IEnumerator LiftAndDescend(CharacterController character)
    {
        Vector3 startPosition = character.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y + bumpHeight, startPosition.z);

        float elapsedTime = 0f;
        float liftDuration = airDuration; // Temps pour soulever l'ennemi

        // Soulever l'ennemi vers la hauteur cible
        while (elapsedTime < liftDuration)
        {
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / liftDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        character.transform.position = targetPosition; // Assurez-vous que la position finale est atteinte

        // Maintenir l'ennemi en l'air pendant une courte durée
        yield return new WaitForSeconds(holdDuration);

        // Descendre l'ennemi en douceur
        Vector3 descentStartPosition = character.transform.position;
        float descendElapsedTime = 0f;

        while (descendElapsedTime < airDuration)
        {
            character.transform.position = Vector3.Lerp(descentStartPosition, startPosition, descendElapsedTime / airDuration);
            descendElapsedTime += Time.deltaTime * descendSpeed;
            yield return null;
        }

        character.transform.position = startPosition; // Assurez-vous que l'ennemi est bien revenu à sa position initiale
    }
}
