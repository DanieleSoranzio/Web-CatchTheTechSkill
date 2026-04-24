using System;
using System.Collections;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class GameManager : Singleton<GameManager>
{
    private bool isGameStarted=false;
    private bool isAudio=true;
    [SerializeField] private Image audioImage;
    [SerializeField] private Sprite audioOffImage;
    [SerializeField] private Sprite audioOnImage;
    
    private void OnEnable()
    {
         EventManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= OnGameOver;
    }

    private IEnumerator Initialize(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventManager.Initialize?.Invoke();
    }
    
    public void GameStarted()
    {
        isGameStarted = true;
        EventManager.OnGameStart?.Invoke();
    }
    private void OnGameOver()
    {
        isGameStarted = false;
        StartCoroutine(Initialize(2f));
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (isGameStarted)
        {
            PauseGame(hasFocus);
        }
    }

    public void OnAudioButton()
    {
        isAudio=!isAudio;
        audioImage.sprite = isAudio?audioOnImage:audioOffImage;
        AudioManager.Instance.ToggleMute();
    }

    private void PauseGame(bool focus)
    {
        Time.timeScale = focus ? 1 : 0;
    }
}
