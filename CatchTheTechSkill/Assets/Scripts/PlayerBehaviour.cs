
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
    
    #endregion
   
    #region Mono
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        sr=GetComponent<SpriteRenderer>();
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity=new Vector2(horizontal*movementSpeed*Time.deltaTime,0);  
    }

    private void OnEnable()
    {
        EventManager.OnSkillCatch += OnSkillCatched;
        EventManager.OnSkillFelt += OnLifeLost; 
    }
    
    private void OnDisable()
    {
        EventManager.OnSkillCatch -= OnSkillCatched;
        EventManager.OnSkillFelt -= OnLifeLost;
    }

    #endregion
    
    #region Player_Controller

    public void MoveAction(InputAction.CallbackContext ctx)
    {
        horizontal=ctx.ReadValue<Vector2>().x;
        if(horizontal!=0)
            sr.flipX= horizontal < 0f? true:false;
    }

    private void OnSkillCatched()
    {
        sr.AnimateColor(this,Color.green,0.15f);
        transform.AnimateScale(this,transform.localScale*1.2f,0.15f);
    }

    private void OnLifeLost()
    {
        sr.AnimateColor(this,Color.red,0.15f);
        transform.AnimateScale(this,transform.localScale*0.8f,0.15f);
    }
    #endregion
}
