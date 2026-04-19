using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraSizeBehaviour : MonoBehaviour
{
    #region Variables
    
    Camera mainCamera;
    [SerializeField] float duration;
    [SerializeField] float magnitude;
    
    #endregion
    #region Mono
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainCamera = Camera.main;
        SetCameraSize();
    }

    private void OnEnable()
    {
        EventManager.OnSkillFelt += Shake;
    }

    private void OnDisable()
    {
        EventManager.OnSkillFelt -= Shake;
    }

    #endregion
    
    #region Methods

    private void SetCameraSize()
    {
        float desiredAspectRatio = 16f/10f;
        float windowSize = (float)Screen.width / Screen.height;
        mainCamera.orthographicSize = 4.8f * (desiredAspectRatio/windowSize);
    }
    public void Shake()
    {
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        Vector3 originalPos = transform.localPosition;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            yield return null;
        }

        transform.localPosition = originalPos;
    }
    #endregion
}
