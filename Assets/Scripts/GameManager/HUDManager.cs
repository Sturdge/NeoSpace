using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI distanceText = null;
    [SerializeField]
    private TextMeshProUGUI indicatorText = null;
    [SerializeField]
    private TextMeshProUGUI timeText = null;
    [SerializeField]
    private TextMeshProUGUI speedText = null;
    [SerializeField]
    private TextMeshProUGUI laserText = null;
    [SerializeField]
    private TextMeshProUGUI boostText = null;

    [Header("Values")]
    [SerializeField]
    private float indicatorThreshold = 30;
    [SerializeField]
    private float indicatorTime = 0.25f;

    [Header("Colours")]
    [SerializeField]
    private Color[] indicatorColours = null;

    private int state = 0;
    private float stateTimer = 0;

    private GameManager gameManager = null;

    private void Awake() => gameManager = GameManager.Instance;

    private void OnEnable()
    {
        gameManager.GameController.OnTimerTick += UpdateHUD;
        gameManager.GameController.OnTimerStopped += ResetDistanceAndSpeed;
    }

    private void ResetDistanceAndSpeed()
    {
        UpdateDistanceText(0);
        UpdateSpeedText(0);
    }

    private void UpdateHUD()
    {
        UpdateTimeText(gameManager.GameController.TimeAlive);
        UpdateSpeedText(gameManager.GameController.PlayerObject.FakeSpeed);
        UpdateDistanceText(gameManager.GameController.CheckDistance());
        UpdateLaserText(gameManager.GameController.PlayerObject.Lasers);
        UpdateBoostText(gameManager.GameController.PlayerObject.SpeedBoosts);
    }

    private void UpdateDistanceText(float value)
    {
        distanceText.text = $"{value:0.00}KM";

        if (value <= indicatorThreshold && value > 0)
            UpdateIndicator();
        else if (indicatorText.IsActive())
            indicatorText.gameObject.SetActive(false);
    }

    private void UpdateIndicator()
    {
        if (!indicatorText.IsActive())
            indicatorText.gameObject.SetActive(true);

        indicatorText.color = indicatorColours[state];

        stateTimer += Time.deltaTime;
        if (stateTimer >= indicatorTime)
        {
            stateTimer = 0;
            state = (state + 1) % 2;
        }
    }

    private void UpdateTimeText(float value) => timeText.text = $"{value:0.0}s";

    private void UpdateSpeedText(float value) => speedText.text = $"{value:0}KM/H";

    private void UpdateLaserText(int value) => laserText.text = $"Lasers: {value}";
    private void UpdateBoostText(int value) => boostText.text = $"Boosts: {value}";
}
