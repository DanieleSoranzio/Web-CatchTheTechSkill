using UnityEngine;

public class FallingSkill : Poolable
{
    Rigidbody2D rb;
    [SerializeField] private float movementSpeed=125;
    #region Mono
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void  Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    #endregion
}
    
