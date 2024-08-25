using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UIBarController : MonoBehaviour
{
    [Range(0, 1)]
    public float percent = 0.5f;

    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image filler;

    private bool alreadyAnimating = false;


    private void Update()
    {
        filler.fillAmount = percent;
    }

    public void SetPercent(float value, bool animate = true)
    {
        if (animate)
        {
            if (alreadyAnimating)
            {
                alreadyAnimating = false;
                StartCoroutine(AnimateChange(value));
            }
        }
        else
        {
            percent = value;
        }
    }

    public void SetPercent(float value, float maxValue, bool animate = true)
    {
        if (animate)
        {
            alreadyAnimating = false;
            StartCoroutine(AnimateChange((value / maxValue)));
        }
        else
        {
            percent = (value / maxValue);
        }
    }

    IEnumerator AnimateChange(float targetValue)
    {
        alreadyAnimating = true;

        float startValue = percent;
        float elapsedTime = 0f;

        while (elapsedTime < animDuration && alreadyAnimating)
        {
            percent = Mathf.Lerp(startValue, targetValue, elapsedTime / animDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        alreadyAnimating = false;

        percent = targetValue;
    }

}