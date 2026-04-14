using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action OnSkillFelt;
    public Action OnSkillCatch;

    private void OnEnable()
    {
         OnSkillFelt += PlayerLossSkill; 
         OnSkillCatch += OnSkillCatched;
    }

    private void OnDisable()
    {
        OnSkillFelt -= PlayerLossSkill;
        OnSkillCatch -= OnSkillCatched;
    }

    private void PlayerLossSkill()
    {
        Debug.Log("PlayerLossSkill");
    }

    private void OnSkillCatched()
    {
        Debug.Log("OnSkillCatched");
    }
}
