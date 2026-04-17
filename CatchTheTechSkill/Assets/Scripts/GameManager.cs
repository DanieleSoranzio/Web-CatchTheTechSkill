using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isGameStarted=false;

    private void OnEnable()
    {
         EventManager.OnSkillCatch += PlayerLossSkill; 
         EventManager.OnSkillCatch += OnSkillCatched;
         EventManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnSkillFelt -= PlayerLossSkill;
        EventManager.OnSkillCatch -= OnSkillCatched;
        EventManager.OnGameOver -= OnGameOver;
    }


    private void PlayerLossSkill()
    {
        //Debug.Log("PlayerLossSkill");
    }

    private void OnSkillCatched()
    {
        //Debug.Log("OnSkillCatched");
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5f); 
        Debug.Log("Game ReStarted");
        isGameStarted = true;
        EventManager.OnGameStart?.Invoke();
        yield return null;
    }
    
    private void OnGameOver()
    {
        StartCoroutine(RestartGame());
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!isGameStarted & hasFocus)
        {
            isGameStarted = true;
            EventManager.OnGameStart?.Invoke();
            Debug.Log("Game Started");
        }
           
        PauseGame(hasFocus);
    }

    private void PauseGame(bool focus)
    {

        Time.timeScale = focus ? 1 : 0;
    }
}
