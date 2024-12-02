using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [Header("TIMERS")]
    [ReadOnlyInspector] public List<Timer> Timers = new List<Timer>();

    [Header("PARAMETERS")]
    [SerializeField] private static float _internalTimeScale = 1f;
    public static float TimeScale { get { return _internalTimeScale; } }

    public bool Paused { get { return TimeScale == 0f; } }

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
            timer.Tick(Time.deltaTime * TimeScale);
        }
    }

    public void PauseTimers()
    {
        SetTimeScale(0f);
    }

    public void SetTimeScale(float scale)
    {
        _internalTimeScale = scale;
    }

    public void TimerEndCallback(Timer timer)
    {
        Timers.Remove(timer);
    }
}
