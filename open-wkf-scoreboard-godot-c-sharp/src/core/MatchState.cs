using System;

namespace KarateScoreboard.Core;

public enum PenaltyLevel
{
    None,
    Chui1,
    Chui2,
    Chui3,
    HansokuChui,
    Hansoku
}

public class Athlete
{
    public string Name { get; set; } = "COMPETIDOR";
    public string Country { get; set; } = "MEX";
    public int Score { get; private set; } = 0;
    public bool HasSenshu { get; set; } = false;
    public PenaltyLevel C1Penalties { get; set; } = PenaltyLevel.None;
    public PenaltyLevel C2Penalties { get; set; } = PenaltyLevel.None;

    public void AddPoints(int points)
    {
        Score += points;
        if (Score < 0) Score = 0;
    }

    public void Reset()
    {
        Score = 0;
        HasSenshu = false;
        C1Penalties = PenaltyLevel.None;
        C2Penalties = PenaltyLevel.None;
    }
}

public class MatchState
{
    public Athlete Aka { get; } = new Athlete { Name = "AKA", Country = "AKA" };
    public Athlete Ao { get; } = new Athlete { Name = "AO", Country = "AO" };
    
    public bool IsMatchOver { get; private set; } = false;

    public event Action? OnScoreChanged;
    public event Action<string>? OnMatchEnded;

    public void AddPointAka(int points)
    {
        if (IsMatchOver) return;
        Aka.AddPoints(points);
        CheckPointGap();
        OnScoreChanged?.Invoke();
    }

    public void AddPointAo(int points)
    {
        if (IsMatchOver) return;
        Ao.AddPoints(points);
        CheckPointGap();
        OnScoreChanged?.Invoke();
    }

    public void SetSenshuAka(bool status)
    {
        if (status) Ao.HasSenshu = false;
        Aka.HasSenshu = status;
        OnScoreChanged?.Invoke();
    }

    public void SetSenshuAo(bool status)
    {
        if (status) Aka.HasSenshu = false;
        Ao.HasSenshu = status;
        OnScoreChanged?.Invoke();
    }

    private void CheckPointGap()
    {
        if (Math.Abs(Aka.Score - Ao.Score) >= 8)
        {
            IsMatchOver = true;
            string winner = Aka.Score > Ao.Score ? Aka.Name : Ao.Name;
            OnMatchEnded?.Invoke($"Victoria por ventaja de puntos: {winner}");
        }
    }

    public void ResetMatch()
    {
        Aka.Reset();
        Ao.Reset();
        IsMatchOver = false;
        OnScoreChanged?.Invoke();
    }
}