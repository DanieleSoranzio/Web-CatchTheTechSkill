using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerBehaviour : MonoBehaviour
{
    #region Variables
    Rigidbody2D rb;
    [SerializeField] private float movementSpeed;
    private int lives=3;
    private float horizontal;
    
    #endregion
   
    #region Mono
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
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
