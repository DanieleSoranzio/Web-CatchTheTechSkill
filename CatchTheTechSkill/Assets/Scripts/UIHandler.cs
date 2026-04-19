using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    #region Variables
    
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] GameObject starterGamePanel;
    [SerializeField] Image livesImage;
    [SerializeField] TextMeshProUGUI starterGameTitle;
    [SerializeField] TextMeshProUGUI starterGameSubTitle;
    [SerializeField] TextMeshProUGUI playerLivesText;
    private Vector3 startPos;
    private Vector3 startScale;

    private int playerScore;
    private int playerLives;
    
    #endregion
    
    #region mono
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startPos=playerScoreText.gameObject.transform.position;
        startScale=playerScoreText.gameObject.transform.localScale;
        Initialize();
    }

    private void OnEnable()
    {
        EventManager.OnGameStart += OnGameStart;
        EventManager.OnGameOver += OnGameLost;
        EventManager.Initialize += Initialize;
        EventManager.OnSkillCatch += OnSkillCatch;
        EventManager.OnSkillFelt+= OnSkillFelt;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= OnGameStart;
        EventManager.OnGameOver -= OnGameLost;
        EventManager.Initialize -= Initialize;
    }

    #endregion
#region UI Methods
    private void Initialize()
    {
        starterGamePanel.SetActive(true);
        playerScoreText.enabled = false;
        playerLivesText.enabled = false;
        livesImage.enabled = false;
        playerScoreText.gameObject.transform.AnimatePosition(this,startPos,0.1f);
        playerScoreText.gameObject.transform.AnimateScale(this,startScale,0.1f,false);
        starterGameTitle.AnimateAlpha(this,0f,1f,pingPong: true,loop:true);
        starterGameSubTitle.AnimateAlpha(this,0f,1f,pingPong: true,loop:true);
        playerScore = 0;
        playerLives = 3;
        playerLivesText.text = playerLives.ToString();
        playerScoreText.text=playerScore.ToString("D4");
    }

    private void OnGameStart()
    {
        starterGamePanel.SetActive(false);
        playerScoreText.enabled = true;
        playerLivesText.enabled = true;
        livesImage.enabled = true;
    }
    private void OnGameLost()
    {
        playerScoreText.gameObject.transform.AnimatePosition(this,Vector3.zero,1f);
        playerScoreText.gameObject.transform.AnimateScale(this,startScale*2f,1f,false);
    }
    
    
    private void OnSkillFelt()
    {
        playerLives--;
        playerLivesText.text = playerLives.ToString();
    }

    private void OnSkillCatch()
    {
        playerScore++;
        playerScoreText.text=playerScore.ToString("D4");
    }

    #endregion
}
