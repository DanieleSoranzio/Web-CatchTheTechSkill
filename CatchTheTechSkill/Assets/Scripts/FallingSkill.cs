using UnityEngine;

public class FallingSkill : Poolable
{
    
    //Data
    bool isCatchable;
    SkillsData data;
    private float movementSpeed=125;
    
    //Components
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    
    #region Mono

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity=new Vector2(0,-1*movementSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            ReturnToPool();
            if(isCatchable)
                EventManager.OnSkillFelt?.Invoke();
        }

        if (other.gameObject.CompareTag(("Player")))
        {
            ReturnToPool();
            if(isCatchable)
                EventManager.OnSkillCatch?.Invoke();
            else
                EventManager.OnSkillFelt?.Invoke();
        }
    }
    
    [Tooltip("Set a new skills data to the object, changing sprite and catchable boolean.")]
    public void SetData(SkillsData newData)
    {
        spriteRenderer.sprite = newData.sprite;
        isCatchable = newData.isCatchable;
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    #endregion
}
    
