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
    [SerializeField] CanvasGroup textCanvasGroup;
    [SerializeField] private Vector3 startPos;
    private Vector3 startScale;

    private int playerScore;
    private int playerLives;
    
    #endregion
    
    #region mono
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startPos=playerScoreText.rectTransform.anchoredPosition;
        startScale=playerScoreText.rectTransform.localScale;
        Initialize();
    }

    private void OnEnable()
    {
        EventManager.OnGameStart += OnGameStart;
        EventManager.OnGameOver += OnGameLost;
        EventManager.Initialize += Initialize;
        EventManager.OnSkillCatch += OnSkillCatch;
        EventManager.OnSkillFelt += OnSkillFelt;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= OnGameStart;
        EventManager.OnGameOver -= OnGameLost;
        EventManager.Initialize -= Initialize;
        EventManager.OnSkillCatch -= OnSkillCatch;
        EventManager.OnSkillFelt -= OnSkillFelt;
    }

    #endregion
#region UI Methods
    private void Initialize()
    {
        starterGamePanel.SetActive(true);
        playerScoreText.enabled = false;
        playerLivesText.enabled = false;
        livesImage.enabled = false;
        textCanvasGroup.gameObject.SetActive(true);
        playerScoreText.rectTransform.AnimatePosition(this,startPos,0.1f);
        playerScoreText.rectTransform.AnimateScale(this,startScale,0.1f,false);
        textCanvasGroup.alpha = 1f;
        textCanvasGroup.AnimateAlpha(this,0f,1f,true,true);
        playerScore = 0;
        playerLives = 3;
        playerLivesText.text = playerLives.ToString();
        playerScoreText.text=playerScore.ToString("D4");
    }

    private void OnGameStart()
    {
        starterGamePanel.SetActive(false);
        textCanvasGroup.gameObject.SetActive(false);
        playerScoreText.enabled = true;
        playerLivesText.enabled = true;
        livesImage.enabled = true;
    }
    
    private void OnGameLost()
    {
        playerScoreText.rectTransform.AnimatePosition(this,Vector3.zero,1f,false);
        playerScoreText.rectTransform.AnimateScale(this,startScale*2f,1f,false);
    }
    private void OnSkillFelt()
    {
        livesImage.rectTransform.AnimateScale(this,livesImage.rectTransform.localScale * 0.8f,0.15f);
        playerLives--;
        playerLivesText.text = playerLives.ToString();
    }

    private void OnSkillCatch()
    {
        playerScoreText.rectTransform.AnimateScale(this,playerScoreText.rectTransform.localScale * 1.2f,0.15f);
        playerScore++;
        playerScoreText.text=playerScore.ToString("D4");
    }

    #endregion
}
