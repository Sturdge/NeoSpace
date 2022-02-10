using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public const string highScoreKey = "HighScore";
    public const string lasersKey = "ExtraLasers";
    public const string speedBoostKey = "ExtraBoosts";
    public const string livesKey = "ExtraLives";

    private GameManager gameManager;

    private void Awake() => gameManager = GameManager.Instance;

    private void OnEnable()
    {
        gameManager.OnGameLoad += CreatePrefs;
    }

    private void OnDisable()
    {
        gameManager.OnGameLoad -= CreatePrefs;
    }

    private void CreatePrefs()
    {
        if (!PlayerPrefs.HasKey(highScoreKey))
            PlayerPrefs.SetFloat(highScoreKey, 0);

        if (!PlayerPrefs.HasKey(lasersKey))
            PlayerPrefs.SetInt(lasersKey, 0);

        if (!PlayerPrefs.HasKey(speedBoostKey))
            PlayerPrefs.SetInt(speedBoostKey, 0);

        if (!PlayerPrefs.HasKey(livesKey))
            PlayerPrefs.SetInt(livesKey, 0);
    }
}

