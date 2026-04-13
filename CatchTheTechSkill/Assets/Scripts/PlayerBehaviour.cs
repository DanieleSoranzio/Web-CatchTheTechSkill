using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerBehaviour : MonoBehaviour
{
    #region Variables
    Rigidbody2D rb;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int lives=3;
    public Poolable poolable;
    private float horizontal;
    
    #endregion
   
    #region Mono
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        poolable.Register();

        StartCoroutine("test");
    }

    void FixedUpdate()
    {
        rb.linearVelocity=new Vector2(horizontal*movementSpeed*Time.deltaTime,0);  
    }

    IEnumerator test()
    {
        
        yield return new WaitForSeconds(5);
        
        Poolable temp = ObjectPooler.Instance.GetPoolable(poolable);
        temp.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        ObjectPooler.Instance.DeletePool(temp);
        yield return new WaitForSeconds(1);
        temp.ReturnToPool();
        yield return null;
    }
    #endregion
    
    #region Player_Controller

    public void MoveAction(InputAction.CallbackContext ctx)
    {
        horizontal=ctx.ReadValue<Vector2>().x;
    }
    
    #endregion
}
