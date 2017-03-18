using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static int score;

    public static int highscore;

    void Start()
    {
        score = PlayerPrefs.GetInt("score");
        PlayerPrefs.SetInt("score", 0);

        highscore = PlayerPrefs.GetInt("highscore", highscore);
        print("Points count:" + score);
    }
    void Update()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        score += pointsToAdd;
        print("Points count:" + score);
    }

    public void Reset()
    {
        score = 0;
    }

    public void SaveCurrentPoints()
    {
        PlayerPrefs.SetInt("score", score);
        print("Points count:" + score);
    }
}
