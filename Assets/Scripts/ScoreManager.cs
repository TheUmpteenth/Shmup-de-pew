using System;

public static class ScoreManager
{
    public static event Action onScoreChanged;
    public static int Score
    {
        get;
        set;
    }

    public static void Reset()
    {
        Score = 0;
        onScoreChanged?.Invoke();
    }

    public static void AddScore(int score)
    {
        Score += score;
        onScoreChanged?.Invoke();
    }
}