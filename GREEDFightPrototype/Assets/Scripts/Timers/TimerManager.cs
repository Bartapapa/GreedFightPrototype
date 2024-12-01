using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [Header("TIMERS")]
    [ReadOnlyInspector] public List<Timer> Timers = new List<Timer>();

    [Header("PARAMETERS")]
    [SerializeField] private float _timeScale = 1f;

    public bool Paused { get { return _timeScale == 0f; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        foreach(Timer timer in Timers)
        {
            timer.Tick(Time.deltaTime * _timeScale);
        }
    }

    public void PauseTimers()
    {
        SetTimeScale(0f);
    }

    public void SetTimeScale(float scale)
    {
        _timeScale = scale;
    }

    public void TimerEndCallback(Timer timer)
    {
        Timers.Remove(timer);
    }
}
