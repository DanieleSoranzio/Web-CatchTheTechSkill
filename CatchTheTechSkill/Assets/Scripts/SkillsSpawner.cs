using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The skill spawner handle everything behind the skills and use the object pooler to get the objects.
/// </summary>
public class SkillsSpawner : MonoBehaviour
{
    #region Variables
    [Header("Data")]
    [SerializeField] private List<SkillsData> catchableSkills;
    [SerializeField] private List<SkillsData> uncatchableSkills;
    private List<SkillsData> chosenList;
    [SerializeField] private FallingSkill skill;

    [Space(10), Header("Setup")] 
    [SerializeField] private GameObject spawnerObject;
    
    [Space(10), Header("Stats")] 
    [Tooltip("Chance to spawn a catchable skill, if it wont it will spawn an uncatchable.")]
    [Range(0, 100),SerializeField] private int catchSkillsChanceInt;
    [SerializeField] private float spawnTimeRate=3f;
    [SerializeField] private float minSpawnTimeRate;
    [SerializeField] private float skillMovementSpeed;
    
    /// <summary>
    /// Private Variables
    /// </summary>
    private float uncatchSkillsChance;
    private float catchSkillsChance;
    private float rangeOfSpawn;

    
    #endregion
    
    #region Mono

    private void Awake()
    {
        skill.Register();
        rangeOfSpawn=spawnerObject.transform.localScale.x/2;
        catchSkillsChance = catchSkillsChanceInt / 100f;
        uncatchSkillsChance = 1-catchSkillsChance;
    }

    void Start()
    {
        StartCoroutine(SpawnSkill());
    }
    
    #endregion
    
    #region Methods
    
    IEnumerator SpawnSkill()
    {
        float spawntimeChance = 0;
        Poolable Obj;
        float randSpawn;
        while (true)
        {
            float timer = 0f;
            while (timer<spawntimeChance)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            spawntimeChance = GetRandomDecimalNumber(spawnTimeRate,minSpawnTimeRate);
            Obj = ObjectPooler.Instance.GetPoolable(skill); 
            if (Obj is FallingSkill spawnedObj)
            {
                spawnedObj.gameObject.SetActive(true);
                chosenList=GetRandomDecimalNumber(1f,0f,true)<catchSkillsChance ? catchableSkills : uncatchableSkills;
                spawnedObj.SetData(chosenList[Random.Range(0, chosenList.Count)]);
                spawnedObj.SetMovementSpeed(skillMovementSpeed);
                randSpawn = Random.Range(-rangeOfSpawn, rangeOfSpawn);
                spawnedObj.gameObject.transform.position = new Vector3(randSpawn,spawnerObject.transform.position.y,spawnerObject.transform.position.z);
                spawnedObj.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45));
            }
            yield return null;
        }
    }

    [Tooltip("Return a random  number in between min and max Amount.")]
    private float GetRandomDecimalNumber(float maxAmount=1f,float minAmount=0f,bool isDecimal=true)
    {
        float x = (Random.Range(minAmount, maxAmount));
        x *= 100;
        x = Mathf.Floor(x);
        if (isDecimal)
            x /= 100;
        return x;
    }
    #endregion
}
