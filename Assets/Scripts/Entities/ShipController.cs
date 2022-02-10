using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    #region Serialized Fields

    [Header("Values")]
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float distanceFromCamera = 10;
    [SerializeField]
    private Vector2 clampOffset = new Vector2();

    [Header("Objects")]
    [SerializeField]
    private GameObject body = null;
    [SerializeField]
    private ParticleSystem deathParticles = null;
    [SerializeField]
    private Laser laser = null;

    #endregion

    #region Fields
    private bool isBoosted = false;
    private bool isAlive = true;
    private float originalFOV = 0;
    private Vector3 mousePosition;
    private Vector3 direction;
    private Vector2 screenBounds;

    #endregion

    #region Component References

    private Rigidbody rigid;
    private GameManager gameManager;

    #endregion

    #region Auto Properties

    public float FakeSpeed { get; private set; }
    public int SpeedBoosts { get; private set; }
    public int Lasers { get; private set; }

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        originalFOV = gameManager.MainCam.fieldOfView;
        screenBounds = gameManager.MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, gameManager.MainCam.transform.position.z));
    }

    private void OnEnable()
    {
        Setup();
        gameManager.OnGameStart += Setup;
        gameManager.OnPlayerDeath += PlayerDeath;
        gameManager.OnGameContinue += Setup;
    }

    private void Update()
    {
        CheckInput();
        IncreaseSpeed(Time.deltaTime);
    }

    void FixedUpdate()
    {
        Movement();
        ClampToScreen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBoost") && gameManager.CurrentState != GameState.PlayerDeath)
            StartCoroutine(DoSpeedBoost(5));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HazardObject"))
            IncreaseSpeed(-5);
    }

    private void OnDisable()
    {
        gameManager.OnGameStart -= Setup;
        gameManager.OnPlayerDeath -= PlayerDeath;
        gameManager.OnGameContinue -= Setup;
    }

    #endregion

    #region Private Methods

    private void Setup()
    {
        isAlive = true;
        isBoosted = false;
        FakeSpeed = 30;
        SpeedBoosts = 3 + PlayerPrefs.GetInt(SaveDataManager.speedBoostKey);
        Lasers = 3 + PlayerPrefs.GetInt(SaveDataManager.lasersKey);
        body.SetActive(true);
    }

    private void CheckInput()
    {
        if (gameManager.CurrentState == GameState.GameStarted)
        {
            if (!GameManager.Instance.IsPaused)
            {
                if (Input.GetMouseButtonDown(1) && !isBoosted && SpeedBoosts > 0)
                    StartCoroutine(DoSpeedBoost(5, true));
                if (Input.GetMouseButtonDown(0) && Lasers > 0 && isAlive)
                    ShootLaser();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.TogglePause(!gameManager.IsPaused);
            }
        }
    }

    private void Movement()
    {
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);
        Vector3 objPosition = gameManager.MainCam.ScreenToWorldPoint(mousePosition);
        direction = (objPosition - transform.position).normalized;
        rigid.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
        transform.rotation = Quaternion.Euler(direction.y * speed, 0, direction.x * speed);
    }

    private void ClampToScreen()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, screenBounds.x + clampOffset.x, -screenBounds.x - clampOffset.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, screenBounds.y + clampOffset.y, -screenBounds.y);
        transform.position = clampedPosition;
    }

    private void IncreaseSpeed(float amount)
    {
        FakeSpeed += amount;
    }

    private void ShootLaser()
    {
        Lasers--;
        SpendPower(SaveDataManager.lasersKey, Lasers);
        AudioManager.Instance.PlayAudio(AudioManager.SoundEffects.Laser);
        Instantiate(laser, transform.position, Quaternion.identity);
    }

    private void SpendPower(string key, int powerValue)
    {
        int newValue = powerValue - 3;

        if (newValue < 0)
            newValue = 0;

        PlayerPrefs.SetInt(key, newValue);
    }

    private void PlayerDeath()
    {
        isAlive = false;
        body.SetActive(false);
        deathParticles.Play();
        AudioManager.Instance.PlayAudio(AudioManager.SoundEffects.PlayerDeath);
        StartCoroutine(DieTimer());
    }

    #endregion

    #region Coroutines

    public IEnumerator DieTimer()
    {
        yield return new WaitForSeconds(deathParticles.main.duration);
        gameManager.SetGameState(GameState.GameOver);
    }

    private IEnumerator DoSpeedBoost(int value, bool hasCost = false)
    {
        isBoosted = true;

        AudioManager.Instance.PlayAudio(AudioManager.SoundEffects.SpeedBoost);

        if (hasCost)
        {
            SpeedBoosts--;
            SpendPower(SaveDataManager.speedBoostKey, SpeedBoosts);
        }

        IncreaseSpeed(value);
        StartCoroutine(LerpTimer(originalFOV, 80, 0.5f));

        float timeElapsed = 0;
        while (timeElapsed < 5)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(LerpTimer(80, originalFOV, 0.5f));
        IncreaseSpeed(-value);

        isBoosted = false;
    }

    private IEnumerator LerpTimer(float start, float end, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            gameManager.MainCam.fieldOfView = Mathf.Lerp(start, end, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameManager.MainCam.fieldOfView = end;
    }

    #endregion
}