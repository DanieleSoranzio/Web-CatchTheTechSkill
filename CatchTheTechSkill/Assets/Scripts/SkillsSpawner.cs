using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class SkillsSpawner : MonoBehaviour
{
    #region Variables
    [Header("Data")]
    [SerializeField] private List<SkillsData> catchableSkills;
    [SerializeField] private List<SkillsData> uncatchableSkills;
    
    [Header("Setup")]
    [Tooltip("Chance to spawn a catchable skill, if it wont it will spawn an uncatchable.")]
    [Range(0, 100),SerializeField] private int catchSkillsChanceInt;
    private float uncatchSkillsChance;
    private float catchSkillsChance;
    
    #endregion
    
    #region Mono

    void Start()
    {
        catchSkillsChance = catchSkillsChanceInt / 100f;
        uncatchSkillsChance = 1-catchSkillsChance;
        StartCoroutine(SpawnSkill());
    }
    
    #endregion
    
    #region Methods
    
    IEnumerator SpawnSkill()
    {
        yield return null;
        float x = (Random.Range(0f, 1f));
        x *= 100;
        x = Mathf.Floor(x);
        x /= 100;
        //returning to times
        Debug.Log(x);
        
        
    }
    #endregion
}
