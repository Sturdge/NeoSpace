using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    #endregion

    #region Auto Properties

    public bool IsPaused { get; private set; } = false;
    public GameState CurrentState { get; private set; } = GameState.Null;
    public GameController GameController { get; private set; } = null;
    public Camera MainCam { get; private set; } = null;

    #endregion

    #region Events

    public delegate void StateChangeHandler();
    public event StateChangeHandler OnGameLoad;
    public event StateChangeHandler OnMainMenu;
    public event StateChangeHandler OnHelpMenu;
    public event StateChangeHandler OnShopMenu;
    public event StateChangeHandler OnCreditsMenu;
    public event StateChangeHandler OnGameStart;
    public event StateChangeHandler OnGameRestart;
    public event StateChangeHandler OnGameOver;
    public event StateChangeHandler OnGameContinue;
    public event StateChangeHandler OnPlayerDeath;

    public delegate void Toggle(bool value);
    public event Toggle OnTogglePause;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        else
            _instance = this;

        MainCam = Camera.main;
        GameController = GetComponent<GameController>();

        DontDestroyOnLoad(gameObject);
    }

    private void Start() => SetGameState(GameState.Null);

    #endregion

    #region Public Methods

    public void SetGameState(GameState state)
    {
        CurrentState = state;

        switch (CurrentState)
        {
            case GameState.Null:
                Cursor.lockState = CursorLockMode.None;
                OnGameLoad?.Invoke();
                SetGameState(GameState.MainMenu);
                break;
            case GameState.MainMenu:
                Cursor.visible = true;
                MainCam.fieldOfView = 60;
                OnMainMenu?.Invoke();
                break;
            case GameState.HelpMenu:
                OnHelpMenu?.Invoke();
                break;
            case GameState.ShopMenu:
                OnShopMenu?.Invoke();
                break;
            case GameState.CreditsMenu:
                OnCreditsMenu?.Invoke();
                break;
            case GameState.GameStarted:
                Cursor.visible = false;
                OnGameStart?.Invoke();
                break;
            case GameState.GameRestarted:
                Cursor.visible = false;
                OnGameRestart?.Invoke();
                break;
            case GameState.GameOver:
                Cursor.visible = true;
                OnGameOver?.Invoke();
                break;
            case GameState.GameContinue:
                Cursor.lockState = CursorLockMode.Confined;
                OnGameContinue?.Invoke();
                break;
            case GameState.PlayerDeath:
                OnPlayerDeath?.Invoke();
                break;
            case GameState.QuitGame:
                Application.Quit();
                break;
        }
    }

    public void TogglePause(bool active)
    {
        int scale = active ? 0 : 1;
        IsPaused = active;
        Time.timeScale = scale;
        Cursor.visible = active;
        OnTogglePause?.Invoke(active);
    }

    #endregion
}
