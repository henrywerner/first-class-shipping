using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _current;

    public static EventManager current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType<EventManager>();
            }
            return _current;
        }
        set => _current = value;
    }

    private void Awake()
    {
        current = this;
    }

    public event Action OnEnemyKilled;

    public void EnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }

    public event Action<float, float> OnScreenShake;

    public void ShakeScreen(float duration, float magnitude)
    {
        OnScreenShake?.Invoke(duration, magnitude);
    }
}
