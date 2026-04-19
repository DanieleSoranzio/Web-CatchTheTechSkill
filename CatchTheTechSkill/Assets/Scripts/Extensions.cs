using UnityEngine;
using System.Collections;
using TMPro;

public static class Extensions
{
    public static void AnimateColor(this SpriteRenderer sr, MonoBehaviour runner, Color targetColor, float duration)
    {
        runner.StartCoroutine(ColorRoutine(sr, targetColor, duration));
    }

    private static IEnumerator ColorRoutine(SpriteRenderer sr, Color targetColor, float duration)
    {
        Color startColor = Color.white;
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

    public static void AnimateScale(this Transform tr, MonoBehaviour runner, Vector3 targetScale, float duration,bool loop=true)
    {
        runner.StartCoroutine(ScaleRoutine(tr, targetScale, duration,loop));
    }

    private static IEnumerator ScaleRoutine(Transform tr, Vector3 targetScale, float duration, bool loop=true)
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

        if (loop)
        {
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

        yield break;
    }
    
    public static void AnimateAlpha(
        this TMP_Text tmp,
        MonoBehaviour runner,
        float targetAlpha,
        float duration,
        bool pingPong = false,
        bool loop = false)
    {
        runner.StartCoroutine(AlphaRoutineTMP(tmp, targetAlpha, duration, pingPong, loop));
    }

    private static IEnumerator AlphaRoutineTMP(
        TMP_Text tmp,
        float targetAlpha,
        float duration,
        bool pingPong,
        bool loop)
    {
        float startAlpha = tmp.alpha;

        do
        {
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                tmp.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                yield return null;
            }

            if (pingPong)
            {
                time = 0f;
                while (time < duration)
                {
                    time += Time.deltaTime;
                    tmp.alpha = Mathf.Lerp(targetAlpha, startAlpha, time / duration);
                    yield return null;
                }
            }

        } while (loop);

        tmp.alpha = pingPong ? startAlpha : targetAlpha;
    }
    public static void AnimatePosition(
        this Transform tr,
        MonoBehaviour runner,
        Vector3 targetPosition,
        float duration,
        bool pingPong = false)
    {
        runner.StartCoroutine(PositionRoutine(tr, targetPosition, duration, pingPong));
    }

    private static IEnumerator PositionRoutine(
        Transform tr,
        Vector3 targetPosition,
        float duration,
        bool pingPong)
    {
        Vector3 startPosition = tr.position;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            tr.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        if (!pingPong)
        {
            tr.position = targetPosition;
            yield break;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            tr.position = Vector3.Lerp(targetPosition, startPosition, t);
            yield return null;
        }

        tr.position = startPosition;
    }
}