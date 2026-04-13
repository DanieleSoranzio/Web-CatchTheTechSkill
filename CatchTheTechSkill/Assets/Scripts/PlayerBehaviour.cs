using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerBehaviour : MonoBehaviour
{
    #region Variables
    Rigidbody2D rb;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int lives=3;
    [SerializeField] List<Poolable> poolables= new List<Poolable>();
    private float horizontal;
    
    #endregion
   
    #region Mono
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        for(int i=0;i<11;i++)
        {
            Poolable temp=ObjectPooler.Instance.GetPoolable(poolables[0]);
            // if (i % 2 == 0)
            // {
            //     temp.gameObject.SetActive(true);
            // }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity=new Vector2(horizontal*movementSpeed*Time.deltaTime,0);  
    }
    
    #endregion
    
    #region Player_Controller

    public void MoveAction(InputAction.CallbackContext ctx)
    {
        horizontal=ctx.ReadValue<Vector2>().x;
    }
    
    #endregion
}
