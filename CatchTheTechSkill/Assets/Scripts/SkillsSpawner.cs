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
    [Range(0, 100),SerializeField] private int minCatchSkillsChanceInt;
    [SerializeField] private float spawnTimeRate=2f;
    [SerializeField] private float minSpawnTimeRate;
    [SerializeField] private float skillMovementSpeed;
    [SerializeField] private float maxSkillMovementSpeed;
    [SerializeField] private int EndGameTimer;
    
    
    /// <summary>
    /// Private Variables
    /// </summary>
    private float catchSkillsChance;
    private float minCatchSkillsChance;
    private float currentCatchSkillsChance;
    private float currentMinSpawnTimeRate;
    private float currentMaxSpawnTimeRate;
    private float currentSkillMvmSpeed;
    private float rangeOfSpawn;
    private float timer;
    private bool isGameStarted=false;

    
    #endregion
    
    #region Mono

    private void Awake()
    {
        skill.Register();
        InitializateStats();
    }

    private void Update()
    {
        if (isGameStarted && timer<EndGameTimer)
        {
            timer+=Time.deltaTime;
            DecreaseCatchSkillsChance();
            DecreaseSpawnSkillsRate();
            IncreaseSkillMvmSpeed();
        }
        else
        {
            InitializateStats();
        }
        
    }

    private void OnEnable()
    {
        EventManager.OnGameStart += StartGame;
        EventManager.OnGameOver += OnGameOver; 
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= StartGame;
        EventManager.OnGameOver -= OnGameOver;
    }

    #endregion
    
    #region Methods

    private void StartGame()
    {
        isGameStarted = true;
        InitializateStats();
        StartCoroutine(SpawnSkill());
    }

    private void InitializateStats()
    {
        rangeOfSpawn=spawnerObject.transform.localScale.x/2;
        catchSkillsChance = catchSkillsChanceInt / 100f;
        currentCatchSkillsChance = catchSkillsChance;
        minCatchSkillsChance = minCatchSkillsChanceInt / 100f;
        currentMaxSpawnTimeRate = spawnTimeRate;
        currentMinSpawnTimeRate = minSpawnTimeRate;
        currentSkillMvmSpeed=skillMovementSpeed;
        timer = 0;
    }
    IEnumerator SpawnSkill()
    {
        float spawntimeChance = 0;
        Poolable Obj;
        float randSpawn;
        while (isGameStarted)
        {
            float tempTimer = 0f;
            while (tempTimer<spawntimeChance)
            {
                tempTimer += Time.deltaTime;
                yield return null;
            }
            spawntimeChance = GetRandomDecimalNumber(currentMaxSpawnTimeRate,currentMinSpawnTimeRate);
            Obj = ObjectPooler.Instance.GetPoolable(skill); 
            if (Obj is FallingSkill spawnedObj)
            {
                spawnedObj.gameObject.SetActive(true);
                chosenList=GetRandomDecimalNumber(1f,0f,true)<currentCatchSkillsChance ? catchableSkills : uncatchableSkills;
                spawnedObj.SetData(chosenList[Random.Range(0, chosenList.Count)]);
                spawnedObj.SetMovementSpeed(currentSkillMvmSpeed);
                randSpawn = Random.Range(-rangeOfSpawn, rangeOfSpawn);
                spawnedObj.gameObject.transform.position = new Vector3(randSpawn,spawnerObject.transform.position.y,spawnerObject.transform.position.z);
                spawnedObj.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45));
            }
            yield return null;
        }
        yield break;
    }

    private void DecreaseCatchSkillsChance()
    {
        float speed = Mathf.Abs((minCatchSkillsChance - catchSkillsChance)) / EndGameTimer;
        currentCatchSkillsChance -= speed* Time.deltaTime;
        currentCatchSkillsChance = Mathf.Clamp(currentCatchSkillsChance, minCatchSkillsChance, catchSkillsChance);
    }
    private void DecreaseSpawnSkillsRate()
    {
         float speed = Mathf.Abs((0.4f - minSpawnTimeRate)) / EndGameTimer;
         currentMinSpawnTimeRate -= speed * Time.deltaTime;
         currentMinSpawnTimeRate = Mathf.Clamp(currentMinSpawnTimeRate, 0.4f, minSpawnTimeRate);
         float speed2 = Mathf.Abs((1f - spawnTimeRate)) / EndGameTimer;
         currentMaxSpawnTimeRate -= speed2 * Time.deltaTime;
         currentMaxSpawnTimeRate = Mathf.Clamp(currentMaxSpawnTimeRate, 1f, spawnTimeRate);
    }
    private void IncreaseSkillMvmSpeed()
    {
        float speed = Mathf.Abs((maxSkillMovementSpeed - skillMovementSpeed)) / EndGameTimer;
        currentSkillMvmSpeed += speed * Time.deltaTime;
        currentSkillMvmSpeed = Mathf.Clamp(currentSkillMvmSpeed, skillMovementSpeed, maxSkillMovementSpeed);
    }
    
    
    private void OnGameOver()
    {
        StopAllCoroutines();
        isGameStarted = false;
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
