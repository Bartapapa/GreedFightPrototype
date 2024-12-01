using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float Duration = 0f;

    private float _elapsedTime = 0f;
    private bool _completed = false;
    private bool _looping = false;
    private TimerManager _manager;

    private Action _onTimerCompleted = null;

    public float ElapsedTime { get { return _elapsedTime; } }
    public float RemainingTime { get { return Duration - _elapsedTime; } }
    public float CompletionRatio { get { return _elapsedTime / Duration; } }
    public bool Looping { get { return _looping; } }

    public Timer (float duration, bool looping, Action onTimerCompleted)
    {
        if (TimerManager.instance == null) return;

        Duration = duration;
        _looping = looping;
        _onTimerCompleted = onTimerCompleted;

        _manager = TimerManager.instance;
        _manager.Timers.Add(this);
    }
    public Timer(float duration) : this(duration, false, null) { }
    public Timer(float duration, Action onTimerCompleted) : this(duration, false, onTimerCompleted) { }
    public Timer(float duration, bool looping) : this(duration, looping, null) { }

    public void Tick(float deltaTime)
    {
        if (_completed) return;

        _elapsedTime += deltaTime;
        if (_elapsedTime >= Duration)
        {
            if (_onTimerCompleted != null) _onTimerCompleted();
            if (_looping)
            {
                _elapsedTime = 0f;
            }
            else
            {
                _completed = true;
                _manager.TimerEndCallback(this);
            }
        }
    }
}
