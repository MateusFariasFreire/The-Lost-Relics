using UnityEngine;
using Random = UnityEngine.Random;

using RengeGames.HealthBars;
using System.Collections;


public class UIBarController : MonoBehaviour
{
    [Range(0, 1)]
    public float percent = 0.5f;

    [SerializeField] private RadialSegmentedHealthBar bar;
    [SerializeField] private float animDuration = 0.5f;

    void Start()
    {
        bar.InnerColor.Value = new Color(1, 1, 1, 1);
        bar.RemoveSegments.Value = 0;
        bar.SetPercent(percent);
    }

    private void Update()
    {
        bar.SetPercent(percent);
    }

    public void SetPercent(float value)
    {
        StartCoroutine(AnimateChange(value));
    }

    public void SetPercent(float value, float maxValue)
    {
        StartCoroutine(AnimateChange((value / maxValue)));
    }

    IEnumerator AnimateChange(float targetValue)
    {
        float startValue = percent;
        float elapsedTime = 0f;

        while (elapsedTime < animDuration)
        {
            percent = Mathf.Lerp(startValue, targetValue, elapsedTime / animDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        percent = targetValue;
    }

}