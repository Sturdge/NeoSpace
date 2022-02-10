using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField]
    private ShipController _playerObject = null;
    [SerializeField]
    private MovingKillPlane movingKillPlane = null;
    [SerializeField]
    private ParticleSystem pulseParticle = null;

    [Header("Settings")]
    [SerializeField]
    private float timeBetweenEvents = 5;

    private GameManager gameManager = null;

    public float TimeElapsed { get; private set; } = 0;
    public float TimeAlive { get; private set; } = 0;
    public Camera MainCam { get; private set; } = null;
    public List<GameObject> ActiveHazards { get; private set; } = new List<GameObject>();

    public ShipController PlayerObject => _playerObject;

    public delegate void TimerEvent();
    public event TimerEvent OnTimerTick;
    public event TimerEvent OnTimerDone;
    public event TimerEvent OnTimerStopped;

    public delegate void ScoreEvent(bool value);
    public event ScoreEvent OnNewHighScore;

    private void Awake()
    {
        MainCam = Camera.main;
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.OnMainMenu += StopGame;
        gameManager.OnGameStart += StartGame;
        gameManager.OnGameRestart += RestartGame;
        gameManager.OnPlayerDeath += CheckHighScore;
        gameManager.OnGameContinue += ContinueGame;
    }

    private void Update()
    {
        if (gameManager.CurrentState == GameState.GameStarted || gameManager.CurrentState == GameState.GameContinue)
        {
            Timer();
        }
    }

    private void OnDisable()
    {
        gameManager.OnMainMenu -= StopGame;
        gameManager.OnGameStart -= StartGame;
        gameManager.OnGameRestart -= RestartGame;
        gameManager.OnPlayerDeath -= CheckHighScore;
        gameManager.OnGameContinue -= ContinueGame;
    }

    private void StartGame()
    {
        TimeAlive = 0;
        TimeElapsed = 0;
        timeBetweenEvents = 5;
        PlayerObject.gameObject.SetActive(true);
        movingKillPlane.gameObject.SetActive(true);
        PlayPulse();
    }

    private void StopGame()
    {
        PlayerObject.gameObject.SetActive(false);
        movingKillPlane.gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        StopGame();
        StartGame();
    }

    private void Timer()
    {
        TimeAlive += Time.deltaTime;
        TimeElapsed += Time.deltaTime;
        OnTimerTick?.Invoke();
        if (TimeElapsed >= timeBetweenEvents)
        {
            OnTimerDone?.Invoke();
            UpdateDelay();
            PlayPulse();
            TimeElapsed = 0;
        }
    }

    private void UpdateDelay()
    {
        timeBetweenEvents = CheckDistance() / 6;

        if (timeBetweenEvents < 2.5f)
            timeBetweenEvents = 2.5f;
        else if (timeBetweenEvents > 5)
            timeBetweenEvents = 5;
    }

    private void PlayPulse()
    {
        pulseParticle.Play();
    }

    private void CheckHighScore()
    {
        movingKillPlane.gameObject.SetActive(false);

        OnTimerStopped?.Invoke();

        bool isNewHighScore = false;
        if (TimeAlive > PlayerPrefs.GetFloat(SaveDataManager.highScoreKey))
        {
            PlayerPrefs.SetFloat(SaveDataManager.highScoreKey, TimeAlive);
            isNewHighScore = true;
        }

        Analytics.CustomEvent($"Time Alive", new Dictionary<string, object>
        {
                {"Time", TimeAlive }
        });

        OnNewHighScore?.Invoke(isNewHighScore);
    }

    private void ContinueGame()
    {
        int livesRemaining = PlayerPrefs.GetInt(SaveDataManager.livesKey);
        livesRemaining--;
        PlayerPrefs.SetInt(SaveDataManager.livesKey, livesRemaining);
        PlayerObject.gameObject.SetActive(true);
        movingKillPlane.gameObject.SetActive(true);
    }

    public float CheckDistance()
    {
        float distance = Vector3.Distance(PlayerObject.transform.position, movingKillPlane.transform.position) - 2;

        if (distance < 0)
            distance = 0;

        return distance;
    }
}