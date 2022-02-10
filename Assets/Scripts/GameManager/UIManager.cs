using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private UICanvas[] canvases = null;

    [Header("Canvases")]
    [SerializeField]
    private Canvas pauseCanvas = null;

    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI highScoreText = null;
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    [SerializeField]
    private TextMeshProUGUI highScoreIndicator = null;
    [SerializeField]
    private TextMeshProUGUI deletedIndicator = null;
    [SerializeField]
    private Button continueButton = null;

    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        gameManager.OnMainMenu += EnableMainMenu;
        gameManager.OnHelpMenu += EnableHelpMenu;
        gameManager.OnShopMenu += EnableShopMenu;
        gameManager.OnCreditsMenu += EnableCreditsMenu;
        gameManager.OnGameOver += EnableGameOverUI;
        gameManager.OnGameContinue += EnableInGameUI;
        gameManager.OnGameStart += EnableInGameUI;
        gameManager.OnTogglePause += TogglePauseUI;
        gameManager.GameController.OnNewHighScore += ToggleHighScoreIndicator;
    }

    private void OnDisable()
    {
        gameManager.OnMainMenu -= EnableMainMenu;
        gameManager.OnHelpMenu -= EnableHelpMenu;
        gameManager.OnShopMenu -= EnableShopMenu;
        gameManager.OnCreditsMenu -= EnableCreditsMenu;
        gameManager.OnGameOver -= EnableGameOverUI;
        gameManager.OnGameStart -= EnableInGameUI;
        gameManager.OnGameContinue -= EnableInGameUI;
        gameManager.OnTogglePause -= TogglePauseUI;
        gameManager.GameController.OnNewHighScore -= ToggleHighScoreIndicator;
    }

    private void EnableMainMenu() => EnableUICanvas(CanvasName.MainMenu);

    private void EnableHelpMenu() => EnableUICanvas(CanvasName.Help);

    private void EnableShopMenu() => EnableUICanvas(CanvasName.Shop);

    private void EnableCreditsMenu() => EnableUICanvas(CanvasName.Credits);

    private void EnableInGameUI() => EnableUICanvas(CanvasName.HeadsUpDisplay);

    private void EnableGameOverUI()
    {
        EnableUICanvas(CanvasName.GameOver);
        UpdateScoreTexts(gameManager.GameController.TimeAlive, PlayerPrefs.GetFloat(SaveDataManager.highScoreKey));
        if (PlayerPrefs.GetInt(SaveDataManager.livesKey) > 0)
            continueButton.gameObject.SetActive(true);
        else
            continueButton.gameObject.SetActive(false);
    }

    public void ToggleHighScoreIndicator(bool value)
    {
        highScoreIndicator.gameObject.SetActive(value);
        highScoreIndicator.enabled = value;
    }

    public void TogglePauseUI(bool active) => pauseCanvas.gameObject.SetActive(active);

    private void EnableUICanvas(CanvasName canvasToEnable)
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].Name == canvasToEnable)
                canvases[i].Canvas.gameObject.SetActive(true);
            else
                canvases[i].Canvas.gameObject.SetActive(false);
        }
    }

    private void UpdateScoreTexts(float scoreValue, float highscoreValue)
    {
        scoreText.text = $"Your Score: {scoreValue:0.0}s";
        highScoreText.text = $"High Score: {highscoreValue:0.0}s";
    }

    public void DeleteHighScore()
    {
        PlayerPrefs.SetFloat(SaveDataManager.highScoreKey, 0);
        ToggleDeletedIndicator(true);
        StartCoroutine(DisableDeletedIndicator());
    }
    private void ToggleDeletedIndicator(bool value)
    {
        deletedIndicator.gameObject.SetActive(value);
        deletedIndicator.enabled = value;
    }

    private IEnumerator DisableDeletedIndicator()
    {
        yield return new WaitForSeconds(1);
        ToggleDeletedIndicator(false);
    }

}
