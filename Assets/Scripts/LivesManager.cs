using System;

public static class LivesManager
{
    public static event Action onLivesChanged;
    
    public static int Lives
    {
        get;
        set;
    }

    public static void Reset()
    {
        Lives = 3;
        onLivesChanged?.Invoke();
    }

    public static void AddLife()
    {
        Lives += 1;
        onLivesChanged?.Invoke();
    }

    public static void RemoveLife()
    {
        Lives -= 1;
        onLivesChanged?.Invoke();
    }
}