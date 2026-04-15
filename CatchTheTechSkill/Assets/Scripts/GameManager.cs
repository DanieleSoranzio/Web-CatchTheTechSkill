using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    
    private void OnEnable()
    {
         EventManager.OnSkillCatch += PlayerLossSkill; 
         EventManager.OnSkillCatch += OnSkillCatched;
    }

    private void OnDisable()
    {
        EventManager.OnSkillFelt -= PlayerLossSkill;
        EventManager.OnSkillCatch -= OnSkillCatched;
    }

    private void PlayerLossSkill()
    {
        //Debug.Log("PlayerLossSkill");
    }

    private void OnSkillCatched()
    {
        //Debug.Log("OnSkillCatched");
    }
}
