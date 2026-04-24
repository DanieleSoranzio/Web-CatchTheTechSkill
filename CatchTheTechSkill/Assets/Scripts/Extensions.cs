using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

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

    public static void AnimateScale(this Transform tr, MonoBehaviour runner, Vector3 startScale,Vector3 targetScale, float duration,bool loop=true)
    {
        runner.StartCoroutine(ScaleRoutine(tr, startScale,targetScale, duration,loop));
    }

    private static IEnumerator ScaleRoutine(Transform tr, Vector3 startScale,Vector3 targetScale, float duration, bool loop=true)
    {
        
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
        this CanvasGroup tmp,
        MonoBehaviour runner,
        float targetAlpha,
        float duration,
        bool pingPong = false,
        bool loop = false)
    {
        runner.StartCoroutine(AlphaRoutineTMP(tmp, targetAlpha, duration, pingPong, loop));
    }

    private static IEnumerator AlphaRoutineTMP(
        CanvasGroup tmp,
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
        if (tr is RectTransform rt)
        {
            runner.StartCoroutine(UIPositionRoutine(rt, targetPosition, duration, pingPong));
        }
        else
        {
            runner.StartCoroutine(PositionRoutine(tr, targetPosition, duration, pingPong));
        }
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
    
    private static IEnumerator UIPositionRoutine(
        RectTransform rt,
        Vector2 targetPosition,
        float duration,
        bool pingPong)
    {
        Vector2 startPosition = rt.anchoredPosition;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            rt.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        if (!pingPong)
        {
            rt.anchoredPosition = targetPosition;
            yield break;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            rt.anchoredPosition = Vector2.Lerp(targetPosition, startPosition, t);
            yield return null;
        }

        rt.anchoredPosition = startPosition;
    }
    public static void Shake(
        this Transform tmp,
        MonoBehaviour runner,
        float magnitude,
        float duration)
    {
        runner.StartCoroutine(ShakeRoutine(tmp, magnitude, duration));
    }

    private static IEnumerator ShakeRoutine(Transform tr, float magnitude, float duration)
    {
        Quaternion originalRot = tr.localRotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float z = Random.Range(-1f, 1f) * magnitude;
            tr.localRotation = originalRot * Quaternion.Euler(0f, 0f, z);
            yield return null;
        }

        tr.localRotation = originalRot;

    }
}