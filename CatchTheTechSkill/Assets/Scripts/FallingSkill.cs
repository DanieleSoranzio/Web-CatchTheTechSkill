using UnityEngine;

public class FallingSkill : Poolable
{
    //Serializable
    [SerializeField] private float movementSpeed=125;
    
    //Data
    bool isCatchable;
    SkillsData data;
    
    //Components
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    
    #region Mono
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void  Start()
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
            GameManager.Instance.OnSkillFelt?.Invoke();
        }

        if (other.gameObject.CompareTag(("Player")))
        {
            ReturnToPool();
            GameManager.Instance.OnSkillCatch?.Invoke();
        }
    }
    
    [Tooltip("Set a new skills data to the object, changing sprite and catchable boolean.")]
    public void SetData(SkillsData data)
    {
        spriteRenderer.sprite = data.sprite;
        isCatchable = data.isCatchable;
    }

    #endregion
}
    
