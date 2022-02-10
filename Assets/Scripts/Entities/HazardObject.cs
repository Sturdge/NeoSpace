using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardObject : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles = null;

    public void Die()
    {
        particles.Play();
        AudioManager.Instance.PlayAudio(AudioManager.SoundEffects.Asteroid);
        gameObject.SetActive(false);
    }
}
