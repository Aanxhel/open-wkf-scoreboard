using Godot;
using System;

namespace KarateScoreboard.Core;

public partial class MatchTimer : Node
{
    public float TimeRemaining { get; private set; } = 180.0f;
    public bool IsRunning { get; private set; } = false;

    public event Action<float>? OnTimeUpdated;
    public event Action? OnTimerFinished;

    public void SetDuration(float seconds)
    {
        TimeRemaining = seconds;
        OnTimeUpdated?.Invoke(TimeRemaining);
    }

    public void ToggleTimer()
    {
        IsRunning = !IsRunning;
    }

    public void StartTimer() => IsRunning = true;
    public void PauseTimer() => IsRunning = false;

    public void ResetTimer(float defaultSeconds = 180.0f)
    {
        IsRunning = false;
        TimeRemaining = defaultSeconds;
        OnTimeUpdated?.Invoke(TimeRemaining);
    }

    public override void _Process(double delta)
    {
        if (!IsRunning) return;

        TimeRemaining -= (float)delta;

        if (TimeRemaining <= 0.0f)
        {
            TimeRemaining = 0.0f;
            IsRunning = false;
            OnTimeUpdated?.Invoke(TimeRemaining);
            OnTimerFinished?.Invoke();
        }
        else
        {
            OnTimeUpdated?.Invoke(TimeRemaining);
        }
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(TimeRemaining / 60.0f);
        int seconds = Mathf.FloorToInt(TimeRemaining % 60.0f);

        if (TimeRemaining < 10.0f && TimeRemaining > 0.0f)
        {
            int tenths = Mathf.FloorToInt((TimeRemaining - Mathf.Floor(TimeRemaining)) * 10.0f);
            return $"{seconds:D2}:{tenths}";
        }

        return $"{minutes:D2}:{seconds:D2}";
    }
}