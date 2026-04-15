using UnityEngine;
using System.Collections;

public static class Extensions
{
    public static void AnimateColor(this SpriteRenderer sr, MonoBehaviour runner, Color targetColor, float duration)
    {
        runner.StartCoroutine(ColorRoutine(sr, targetColor, duration));
    }

    private static IEnumerator ColorRoutine(SpriteRenderer sr, Color targetColor, float duration)
    {
        Color startColor = sr.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            sr.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            sr.color = Color.Lerp(targetColor, startColor, t);
            yield return null;
        }

        sr.color = startColor;
    }

    public static void AnimateScale(this Transform tr, MonoBehaviour runner, Vector3 targetScale, float duration)
    {
        runner.StartCoroutine(ScaleRoutine(tr, targetScale, duration));
    }

    private static IEnumerator ScaleRoutine(Transform tr, Vector3 targetScale, float duration)
    {
        Vector3 startScale = tr.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            tr.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            tr.localScale = Vector3.Lerp(targetScale, startScale, t);
            yield return null;
        }

        tr.localScale = startScale;
    }
}