using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private static EventSystem _instance;
    public static EventSystem Instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        else
            _instance = this;
    }

    public event Action OnPlayerDied;
    public void PlayerDied() => OnPlayerDied?.Invoke();
}
