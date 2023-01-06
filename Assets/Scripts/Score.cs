using UnityEngine;

public static class Score 
{
    private static int _highscore, _score, _gold;

    public static int Hidhscore => _highscore;
    public static int TotalEarned => _score;
    public static int Gold => _gold;

    public static void SetDefaultScore()
    {
        _highscore = PlayerPrefs.GetInt("highscore", 0);
        _score = 0;
        _gold = 100;

        EventManager.SendGoldChanged(_gold);
        EventManager.SendHighscoreChanged(_highscore);
    }

    public static void GoldIncrease(int value)
    {
        _gold += value;
        TotalEarnedIncease(value);

        EventManager.SendGoldChanged(_gold);
    }

    public static void GoldDecrease(int value)
    {
        _gold -= value;
        EventManager.SendGoldChanged(_gold);
        if (_gold <= 0)
        {
            EventManager.SendGameOver();
        }
    }

    private static void TotalEarnedIncease(int value)
    {
        _score += value;
        //передать в ui         //Задаётся вместе с gold

        if (_score > _highscore)
        {
            _highscore = _score;
            EventManager.SendHighscoreChanged(_highscore);
        }
    }
}
