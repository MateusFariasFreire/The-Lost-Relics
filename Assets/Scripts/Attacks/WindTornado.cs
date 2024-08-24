using System.Collections;
using UnityEngine;

public class WindTornado : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float bumpHeight = 5f;
    [SerializeField] private float liftSpeed = 2f; 

    [SerializeField] private GameObject windEffect;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.GetComponent<CharacterController>();

        if (character != null)
        {
            StartCoroutine(Lift(character));
        }
    }

    private IEnumerator Lift(CharacterController character)
    {
        Vector3 startPosition = character.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y + bumpHeight, startPosition.z);

        float elapsedTime = 0f;
        float liftDuration = bumpHeight / liftSpeed;

        while (elapsedTime < liftDuration)
        {
            float progress = elapsedTime / liftDuration;

            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            character.transform.position = Vector3.Lerp(startPosition, targetPosition, easedProgress);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        character.transform.position = targetPosition;
    }

}
