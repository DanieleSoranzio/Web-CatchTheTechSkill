using UnityEngine;

public class CameraSizeBehaviour : MonoBehaviour
{
    #region Variables
    Camera mainCamera;
    #endregion
    #region Mono
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainCamera = Camera.main;
        SetCameraSize();
    }
    #endregion
    
    #region Methods

    private void SetCameraSize()
    {
        float desiredAspectRatio = 16f/10f;
        float windowSize = (float)Screen.width / Screen.height;
        mainCamera.orthographicSize = 4.8f * (desiredAspectRatio/windowSize);
    }
    #endregion
}
