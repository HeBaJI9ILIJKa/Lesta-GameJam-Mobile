using UnityEngine.Events;

public static class EventManager
{
    public static readonly UnityEvent<Enemy> OnEnemyKilled = new UnityEvent<Enemy>();
    public static readonly UnityEvent<Enemy> OnEnemyReturnToPool = new UnityEvent<Enemy>();

    public static readonly UnityEvent OnGameOver = new UnityEvent();
    public static readonly UnityEvent<int> OnGoldChanged = new UnityEvent<int>();
    public static readonly UnityEvent<int> OnHighscoreChanged = new UnityEvent<int>();

    public static readonly UnityEvent OnOrderChanged = new UnityEvent();

    public static readonly UnityEvent<AbilityType> OnAbilityChanged = new UnityEvent<AbilityType>();

    public static void SendEnemyKilled(Enemy enemy) => OnEnemyKilled?.Invoke(enemy);
    public static void SendEnemyReturnToPool(Enemy enemy) => OnEnemyReturnToPool?.Invoke(enemy);


    public static void SendGameOver() => OnGameOver?.Invoke();
    public static void SendGoldChanged(int value) => OnGoldChanged?.Invoke(value);
    public static void SendHighscoreChanged(int value) => OnHighscoreChanged?.Invoke(value);

    public static void SendOrderChanged() => OnOrderChanged?.Invoke();

    public static void SendAbilityChanged(AbilityType type) => OnAbilityChanged?.Invoke(type);
}

