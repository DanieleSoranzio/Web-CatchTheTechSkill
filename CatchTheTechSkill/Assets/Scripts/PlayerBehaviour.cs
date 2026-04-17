
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerBehaviour : MonoBehaviour
{
    #region Variables
    
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private float movementSpeed;
    private int lives=3;
    private float horizontal;
    private bool canMove=false;
    private Vector3 startPos;
    
    #endregion
   
    #region Mono
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        sr=GetComponent<SpriteRenderer>();
        startPos=transform.position;
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity=new Vector2(horizontal*movementSpeed*Time.deltaTime,0);  
    }

    private void OnEnable()
    {
        EventManager.OnSkillCatch += OnSkillCatched;
        EventManager.OnSkillFelt += OnLifeLost; 
        EventManager.OnGameStart += OnGameStart;
        EventManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnSkillCatch -= OnSkillCatched;
        EventManager.OnSkillFelt -= OnLifeLost;
        EventManager.OnGameStart -= OnGameStart;
        EventManager.OnGameOver -= OnGameOver; 
    }
    
    #endregion
    
    #region Player_Controller

    public void MoveAction(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            horizontal=ctx.ReadValue<Vector2>().x;
            if(horizontal!=0)
                sr.flipX= horizontal < 0f? true:false;
        }
        
    }
    
    #endregion
    
    private void OnSkillCatched()
    {
        sr.AnimateColor(this,Color.green,0.15f);
        transform.AnimateScale(this,transform.localScale*1.2f,0.15f);
    }

    private void OnLifeLost()
    {
        sr.AnimateColor(this,Color.red,0.15f);
        transform.AnimateScale(this,transform.localScale*0.8f,0.15f);
        lives--;
        if (lives == 0)
        {
            EventManager.OnGameOver?.Invoke();
        }
    }
    
    private void OnGameOver()
    {
        canMove = false;
    }
    private void OnGameStart()
    {
        lives = 3;
        canMove = true;
        transform.position=startPos;
    }
   
}
