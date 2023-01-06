using UnityEngine;

public static class PauseManager
{
    public static void Pause()
    {
        if (GameParameters.GameRunning)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        GameParameters.GameRunning = !GameParameters.GameRunning;
    }
}
