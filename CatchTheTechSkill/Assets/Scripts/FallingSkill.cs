using System;
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
                GameManager.Instance.OnSkillFelt?.Invoke();
        }

        if (other.gameObject.CompareTag(("Player")))
        {
            ReturnToPool();
            if(isCatchable)
                GameManager.Instance.OnSkillCatch?.Invoke();
            else
                GameManager.Instance.OnSkillFelt?.Invoke();
        }
    }
    
    [Tooltip("Set a new skills data to the object, changing sprite and catchable boolean.")]
    public void SetData(SkillsData newData)
    {
        spriteRenderer.sprite = newData.sprite;
        isCatchable = newData.isCatchable;
    }

    #endregion
}
    
