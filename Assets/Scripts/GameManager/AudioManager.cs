using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundEffects
    {
        Asteroid,
        PlayerDeath,
        Laser,
        SpeedBoost

    }

    #region Singleton Instance

    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    #endregion

    [SerializeField]
    private AudioSource[] sfx = null;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        else
            _instance = this;
    }

    public void PlayAudio(SoundEffects sound)
    {
        AudioSource audioToPlay = sfx[(int)sound];
        audioToPlay.PlayOneShot(audioToPlay.clip);
    }
}