using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTornado : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float bumpForce = 5f;
    [SerializeField] private float fadeOutTime = 1f;

    [SerializeField] GameObject windEffect;


    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            StartCoroutine(FadeOut(fadeOutTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Rigidbody>().AddForce(transform.up * bumpForce, ForceMode.Impulse);
    }

    IEnumerator FadeOut(float fadeOut)
    {
        Vector3 startScale = windEffect.transform.localScale;
        Vector3 endScale = new Vector3(0, 0, 0);

        float currentTime = 0.0f;
        while (true)
        {
            if (currentTime >= fadeOut)
            {
                break;
            }
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / fadeOut);
            yield return null;
        }

        Destroy(gameObject);
    }
}
