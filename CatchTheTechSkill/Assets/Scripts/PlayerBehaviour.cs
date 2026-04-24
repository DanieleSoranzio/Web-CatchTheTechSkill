
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class PlayerBehaviour : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private TextMeshPro comboText;
    [SerializeField] private AudioClip catchAudio;
    [SerializeField] private AudioClip missAudio;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int lives=3;
    private float horizontal;
    private bool canMove=false;
    private Vector3 startPos;
    private int combo;
    
    #endregion
   
    #region Mono
    void Awake()
    {
        //comboText=GetComponentInChildren<TextMeshPro>();
        comboText.gameObject.SetActive(false);
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
        EventManager.Initialize += Initialize;
        EventManager.OnSkillCatch += OnSkillCatched;
        EventManager.OnSkillFelt += OnLifeLost; 
        EventManager.OnGameStart += OnGameStart;
        EventManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.Initialize-= Initialize;
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
    #region Methods
    private void Initialize()
    {
        lives=3;
        combo = 0;
        transform.position = startPos;
        comboText.gameObject.SetActive(false);
    }
    private void OnSkillCatched()
    {
        combo++;
        if (combo % 2 == 0)
        {
            StartCoroutine(ShowCombo());
        }
        sr.AnimateColor(this,Color.green,0.15f);
        transform.AnimateScale(this,new Vector3(2f,2f,0),transform.localScale*1.2f,0.15f);
        AudioManager.Instance.PlaySFX(catchAudio,0.15f,Random.Range(0.8f,1f));
    }

    private void OnLifeLost()
    {
        combo = 0;
        sr.AnimateColor(this,Color.red,0.15f);
        transform.AnimateScale(this,new Vector3(2f,2f,0),transform.localScale*0.8f,0.15f);
        lives--;
        AudioManager.Instance.PlaySFX(missAudio,0.15f,Random.Range(0.8f,1f));
        if (lives == 0)
        {
            EventManager.OnGameOver?.Invoke();
        }
    }
    
    private void OnGameOver()
    {
        canMove = false;
        horizontal=0;
    }
    private void OnGameStart()
    {
        horizontal=0;
        canMove = true;
    }

    private IEnumerator ShowCombo()
    {
        comboText.text= "X"+combo.ToString() + " combo!";
        comboText.gameObject.SetActive(true);
        comboText.transform.Shake(this,5f,0.6f);
        float tempScale = combo * 0.1f;
        tempScale += 1;
        tempScale = Mathf.Clamp(tempScale,0f,2f);
        comboText.transform.AnimateScale(this,comboText.transform.localScale,comboText.transform.localScale*tempScale,0.3f,false);
        yield return new WaitForSeconds(0.6f);
        comboText.gameObject.SetActive(false);
        comboText.transform.localScale=new Vector3(1f,1f,0);
    }
   #endregion
}
