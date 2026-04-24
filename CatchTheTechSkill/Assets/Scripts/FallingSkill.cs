using System;
using UnityEngine;

public class FallingSkill : Poolable
{
    
    //Data
    bool isCatchable;
    SkillsData data;
    private float movementSpeed;
    [SerializeField] private LostSkillParticle lostSkillParticles;
    [SerializeField] private LostSkillParticle ExplosionParticles;
    
    //Components
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Collider2D coll;
    
    #region Mono

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        EventManager.OnGameOver += GetBackToPool;
        coll.enabled = true;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= GetBackToPool;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity=new Vector2(0,-1*movementSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        coll.enabled = false;
        if (other.gameObject.CompareTag("Finish"))
        {
            if (isCatchable)
            {

                Poolable obj = ObjectPooler.Instance.GetPoolable(lostSkillParticles);
                if (obj is LostSkillParticle particle)
                {
                    particle.particlePos = transform.position;
                    particle.gameObject.SetActive(true);
                    particle.PlayParticleEffect();
                }
                EventManager.OnSkillFelt?.Invoke();
            }
            else
            {
                Poolable obj = ObjectPooler.Instance.GetPoolable(ExplosionParticles);
                if (obj is LostSkillParticle particle)
                {
                    particle.particlePos = transform.position;
                    particle.gameObject.SetActive(true);
                    particle.PlayParticleEffect();
                }
            }
            ReturnToPool();
        }

        if (other.gameObject.CompareTag(("Player")))
        {
            if(isCatchable)
                EventManager.OnSkillCatch?.Invoke();
            else
                EventManager.OnSkillFelt?.Invoke();
            
            ReturnToPool();
        }
        
    }
    #endregion
    
    #region Methods
    
    [Tooltip("Set a new skills data to the object, changing sprite and catchable boolean.")]
    public void SetData(SkillsData newData)
    {
        data = newData;
        spriteRenderer.sprite = data.sprite;
        isCatchable = data.isCatchable;
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    private void GetBackToPool()
    {
        ReturnToPool();
    }

    #endregion
}
    
