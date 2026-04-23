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
        transform.Shake(this,magnitude,duration);
    }
    #endregion
}
