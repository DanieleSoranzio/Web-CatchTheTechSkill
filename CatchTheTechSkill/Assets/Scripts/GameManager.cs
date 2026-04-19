using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isGameStarted=false;
    
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
        PauseGame(hasFocus);
    }

    private void PauseGame(bool focus)
    {
        Time.timeScale = focus ? 1 : 0;
    }
}
